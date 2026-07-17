import { chromium } from 'playwright-core';
import fs from 'node:fs';
import path from 'node:path';

const baseURL = process.env.STAINER_BASE_URL || 'http://127.0.0.1:5211';
const stamp = Date.now().toString(36).toUpperCase();
const names = {
  he: `UI验收 HE ${stamp}`,
  ihc: `UI验收 IHC ${stamp}`
};

let browser;
main().catch(error => {
  console.error(`[workflow-browser] FAILED: ${error.stack || error.message || error}`);
  process.exitCode = 1;
}).finally(async () => {
  if (browser) await browser.close().catch(() => {});
});

async function main() {
  console.log(`[workflow-browser] launching ${resolveBrowserExecutable()}`);
  browser = await chromium.launch({
    headless: true,
    executablePath: resolveBrowserExecutable(),
    args: ['--disable-gpu', '--disable-background-networking']
  });
  const context = await browser.newContext({ baseURL, viewport: { width: 1920, height: 1080 } });
  const login = await context.request.post('/api/login', {
    data: { username: 'admin', password: '123456', role: 'admin' }
  });
  assert(login.ok(), `Admin login failed: ${login.status()} ${await login.text()}`);

  const page = await context.newPage();
  const pageErrors = [];
  page.on('pageerror', error => pageErrors.push(error.message));
  let delayedWorkflowList = false;
  await page.route('**/api/workflows', async route => {
    if(!delayedWorkflowList) {
      delayedWorkflowList = true;
      await new Promise(resolve => setTimeout(resolve, 700));
    }
    await route.continue();
  });
  console.log('[workflow-browser] opening control console');
  await page.goto('/control-console');
  await page.waitForFunction(() => document.getElementById('loginScreen')?.classList.contains('hidden'), null, { timeout: 15000 });
  console.log('[workflow-browser] checking delayed-load tab race');
  await page.evaluate(() => {
    showSideTab('config');
    showSideTab('production');
    showSideTab('config');
  });
  await page.waitForSelector('#configPane.active #configNewProfileBtn', { state: 'attached', timeout: 15000 });
  const configDom = await page.evaluate(() => ({
    activePaneCount: document.querySelectorAll('.workspace-pane.active').length,
    renderedSection: document.getElementById('configModule')?.dataset.renderedSection || '',
    hasSummary: !!document.querySelector('#configProfileFold > summary'),
    hasWorkbench: !!document.getElementById('configFlowFold'),
    summaryText: document.querySelector('#configProfileFold > summary')?.textContent?.trim() || '',
    summaryRect: (() => {
      const rect = document.querySelector('#configProfileFold > summary')?.getBoundingClientRect();
      return rect ? { width: rect.width, height: rect.height } : null;
    })(),
    workbenchRect: (() => {
      const rect = document.getElementById('configFlowFold')?.getBoundingClientRect();
      return rect ? { width: rect.width, height: rect.height } : null;
    })()
  }));
  assert(configDom.activePaneCount === 1 && configDom.renderedSection === 'files'
    && configDom.hasSummary && configDom.hasWorkbench
    && configDom.summaryText.includes('当前配置')
    && configDom.summaryRect?.width > 0 && configDom.summaryRect?.height > 0
    && configDom.workbenchRect?.width > 0 && configDom.workbenchRect?.height > 0,
  `Configuration pane was incomplete after delayed tab switching: ${JSON.stringify(configDom)}`);

  console.log('[workflow-browser] creating and publishing HE workflow');
  const heId = await createAndPublish(page, context, 'HE', '#configNewHeProfileBtn', names.he);
  console.log('[workflow-browser] creating and publishing IHC workflow');
  const ihcId = await createAndPublish(page, context, 'IHC', '#configNewProfileBtn', names.ihc, {
    targetTempC: 41.5,
    dabA: 2,
    dabB: 1,
    water: 17
  });

  console.log('[workflow-browser] applying workflows to channels');
  await applyToChannel(page, 1, heId, names.he);
  await applyToChannel(page, 2, ihcId, names.ihc);

  const heDetail = await getJson(context, `/api/workflow-versions/${encodeURIComponent(heId)}`);
  const ihcDetail = await getJson(context, `/api/workflow-versions/${encodeURIComponent(ihcId)}`);
  assert(heDetail.status === 'Published' && heDetail.workflowType === 'HE', 'HE UI script was not published.');
  assert(ihcDetail.status === 'Published' && ihcDetail.workflowType === 'IHC', 'IHC UI script was not published.');
  const rules = JSON.parse(ihcDetail.planningRulesJson);
  assert(rules.targetTempC === 41.5, `IHC target temperature did not round-trip: ${ihcDetail.planningRulesJson}`);
  assert(rules.dabRatio?.a === 2 && rules.dabRatio?.b === 1 && rules.dabRatio?.pureWater === 17,
    `IHC DAB ratio did not round-trip: ${ihcDetail.planningRulesJson}`);
  assert(ihcDetail.steps.some(step => JSON.parse(step.legacyParametersJson || '{}').ui), 'Step UI metadata was not persisted.');
  assert(pageErrors.length === 0, `Page errors occurred: ${JSON.stringify(pageErrors)}`);

  console.log(`[workflow-browser] PASS HE=${heId} IHC=${ihcId} base=${baseURL}`);
}

async function createAndPublish(page, context, workflowType, createButton, name, rules = null) {
  await openConfigFiles(page);
  const beforeIds = new Set(await workflowVersionIds(context));
  await openProfileDrawer(page);
  await page.click(createButton);
  let workflowVersionId = '';
  await waitUntil(async () => {
    const ids = await workflowVersionIds(context);
    workflowVersionId = ids.find(id => !beforeIds.has(id)) || '';
    if(!workflowVersionId) return false;
    return await page.evaluate(({ workflowVersionId, workflowType }) => {
      const select = document.getElementById('configEditProfileSelect');
      const meta = document.getElementById('configCurrentProfileMeta')?.textContent || '';
      return select?.value === workflowVersionId
        && meta.includes(workflowType)
        && meta.includes('Draft')
        && !document.getElementById('configProfileNameInput')?.disabled;
    }, { workflowVersionId, workflowType });
  }, `${workflowType} workflow draft editor refresh`, 30000);

  await openProfileDrawer(page);
  assert(workflowVersionId, `${workflowType} draft id is missing from the editor.`);
  await page.fill('#configProfileNameInput', name);
  await page.fill('#configProfileDescInput', `${workflowType} workflow saved by browser acceptance.`);
  await page.click('#configRenameQuickBtn');
  await waitUntil(async () => (await getJson(context, `/api/workflow-versions/${encodeURIComponent(workflowVersionId)}`)).name === name,
    `${workflowType} basic information save`);
  await page.waitForFunction(({ workflowVersionId, name }) => {
    return document.getElementById('configEditProfileSelect')?.value === workflowVersionId
      && document.getElementById('configCurrentProfileName')?.textContent?.trim() === name;
  }, { workflowVersionId, name }, { timeout: 15000 });

  await page.fill('#configStepLabelInput', `${workflowType} 验收第一步`);
  await page.fill('#configStepToleranceInput', '9');
  await page.check('#configStepImmediateInput');
  await page.click('#configSaveStepBtn');
  await waitUntil(async () => {
    const detail = await getJson(context, `/api/workflow-versions/${encodeURIComponent(workflowVersionId)}`);
    const ui = JSON.parse(detail.steps[0]?.legacyParametersJson || '{}').ui;
    return detail.steps[0]?.stepName === `${workflowType} 验收第一步` && ui?.toleranceSec === 9 && ui?.immediateAfterPrev === true;
  }, `${workflowType} step metadata save`);
  await waitForLog(page, `已保存数据库草稿第 1 层：${workflowType} 验收第一步`);

  if (rules) {
    await page.click('#configTabRules');
    await page.waitForSelector('#configRulesCard #configSaveRulesBtn');
    await page.fill('#configTargetTempInput', String(rules.targetTempC));
    await page.fill('#configDabAInput', String(rules.dabA));
    await page.fill('#configDabBInput', String(rules.dabB));
    await page.fill('#configDabWaterInput', String(rules.water));
    await page.check('#configAllowMultiPrimaryInput');
    await page.fill('#configProfileNotesInput', '浏览器验收规则');
    await page.click('#configSaveRulesBtn');
    await waitUntil(async () => {
      const detail = await getJson(context, `/api/workflow-versions/${encodeURIComponent(workflowVersionId)}`);
      const stored = JSON.parse(detail.planningRulesJson);
      return stored.targetTempC === rules.targetTempC
        && stored.dabRatio?.a === rules.dabA
        && stored.dabRatio?.b === rules.dabB
        && stored.dabRatio?.pureWater === rules.water;
    }, `${workflowType} planning rule save`);
    await waitForLog(page, `已保存数据库规划规则：${name}`, 30000);
  }

  await openConfigFiles(page);
  await openProfileDrawer(page);
  await page.click('#configPublishProfileBtn');
  await waitUntil(async () => (await getJson(context, `/api/workflow-versions/${encodeURIComponent(workflowVersionId)}`)).status === 'Published',
    `${workflowType} publish`, 25000);
  await page.waitForFunction(() => {
    const meta = document.getElementById('configCurrentProfileMeta')?.textContent || '';
    return meta.includes('Published') && document.getElementById('configProfileNameInput')?.disabled === true;
  }, null, { timeout: 15000 });
  return workflowVersionId;
}

async function applyToChannel(page, channelId, workflowVersionId, name) {
  await page.evaluate(id => showChannelDetail(id), channelId);
  const selector = `#statusChannel${channelId}ConfigSelect`;
  const button = `#statusChannel${channelId}SetDefaultConfigBtn`;
  await page.waitForSelector(selector);
  if(channelId === 1) {
    await page.waitForTimeout(5500);
    assert(await page.locator(selector).isVisible(), 'Channel workflow selector was removed by realtime snapshot refresh.');
  }
  await page.selectOption(selector, workflowVersionId);
  await page.click(button);
  await page.waitForFunction(({ channelId, workflowVersionId, name }) => {
    const select = document.getElementById(`statusChannel${channelId}ConfigSelect`);
    const preview = document.getElementById(`statusChannel${channelId}ProfilePreview`)?.textContent || '';
    return select?.value === workflowVersionId && preview.includes(name);
  }, { channelId, workflowVersionId, name }, { timeout: 15000 });
}

async function openConfigFiles(page) {
  await page.evaluate(() => {
    showSideTab('config');
    renderConfigPane('files');
  });
  await page.waitForSelector('#configPane.active #configCurrentProfileMeta', { timeout: 15000 });
}

async function openProfileDrawer(page) {
  await page.evaluate(() => {
    const drawer = document.getElementById('configProfileFold');
    if (drawer) drawer.open = true;
  });
  await page.waitForSelector('#configProfileFold[open] #configProfileNameInput');
}

async function workflowVersionCount(context) {
  return (await workflowVersionIds(context)).length;
}

async function workflowVersionIds(context) {
  const workflows = await getJson(context, '/api/workflows');
  return workflows.flatMap(workflow => (workflow.versions || []).map(version => version.id || version.workflowVersionId)).filter(Boolean);
}

async function getJson(context, url) {
  const response = await context.request.get(url);
  assert(response.ok(), `GET ${url} failed: ${response.status()} ${await response.text()}`);
  return await response.json();
}

async function waitUntil(predicate, label, timeoutMs = 15000) {
  const deadline = Date.now() + timeoutMs;
  let lastError;
  while (Date.now() < deadline) {
    try {
      if (await predicate()) return;
    } catch (error) {
      lastError = error;
    }
    await new Promise(resolve => setTimeout(resolve, 150));
  }
  throw new Error(`Timed out waiting for ${label}${lastError ? `: ${lastError.message}` : ''}.`);
}

async function waitForLog(page, expected, timeoutMs = 15000) {
  await page.waitForFunction(text => document.getElementById('logList')?.textContent?.includes(text), expected, { timeout: timeoutMs });
}

function resolveBrowserExecutable() {
  const configured = process.env.PLAYWRIGHT_CHROME_EXECUTABLE;
  if (configured && fs.existsSync(configured)) return configured;
  const candidates = [
    path.join(process.env.ProgramFiles || 'C:\\Program Files', 'Google', 'Chrome', 'Application', 'chrome.exe'),
    path.join(process.env['ProgramFiles(x86)'] || 'C:\\Program Files (x86)', 'Google', 'Chrome', 'Application', 'chrome.exe'),
    path.join(process.env.ProgramFiles || 'C:\\Program Files', 'Microsoft', 'Edge', 'Application', 'msedge.exe'),
    path.join(process.env['ProgramFiles(x86)'] || 'C:\\Program Files (x86)', 'Microsoft', 'Edge', 'Application', 'msedge.exe')
  ];
  const found = candidates.find(candidate => fs.existsSync(candidate));
  assert(found, 'No Chrome or Edge executable was found.');
  return found;
}

function assert(condition, message) {
  if (!condition) throw new Error(message);
}
