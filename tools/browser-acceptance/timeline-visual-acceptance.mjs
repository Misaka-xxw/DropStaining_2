import { chromium } from 'playwright-core';
import fs from 'node:fs';
import path from 'node:path';

const baseURL = process.env.STAINER_BASE_URL || 'http://127.0.0.1:5205';
let browser;

main().catch(error => {
  console.error(`[timeline-browser] FAILED: ${error.stack || error.message || error}`);
  process.exitCode = 1;
}).finally(async () => {
  if (browser) await browser.close().catch(() => {});
});

async function main() {
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
  await page.goto('/control-console');
  await page.waitForFunction(() => document.getElementById('loginScreen')?.classList.contains('hidden'), null, { timeout: 15000 });
  await page.waitForFunction(() => typeof runtimeStepTarget === 'function' && !!findTarget('R21'), null, { timeout: 15000 });

  await page.route('**/api/operator/snapshot', route => route.fulfill({
    status: 200,
    contentType: 'application/json',
    body: JSON.stringify({ channels: [
      { experimentType: 'HE', workflowSelectionStatus: 'Selected', slides: [{ stainingTaskId: 'task-he-1' }] },
      { experimentType: 'IHC', workflowSelectionStatus: 'Selected', slides: [{ stainingTaskId: 'task-ihc-1' }, { stainingTaskId: 'task-ihc-2' }] }
    ] })
  }));
  const taskPlan = await page.evaluate(() => collectStainingTaskPlan());
  assert(taskPlan.taskIds.join(',') === 'task-he-1,task-ihc-1,task-ihc-2',
    `Run task collection was wrong: ${JSON.stringify(taskPlan)}`);
  assert(taskPlan.dabTaskIds.join(',') === 'task-ihc-1,task-ihc-2',
    `DAB task collection included a non-IHC task: ${JSON.stringify(taskPlan)}`);

  const result = await page.evaluate(async () => {
    stopRealtimeSync();
    if(_snapshotLoadPromise) await _snapshotLoadPromise;
    stopRealtimeSync();
    if(_snapshotPollTimer) {
      clearTimeout(_snapshotPollTimer);
      _snapshotPollTimer = null;
    }

    const steps = ['B-01', 'B-02', 'B-03'].map((slotCode, index) => ({
      id: `acceptance-step-${index + 1}`,
      stepNo: index + 1,
      actionType: 'Dispense',
      majorStepCode: 'PRIMARY_ANTIBODY',
      reagentCode: 'P01',
      slotCode,
      targetPointCode: slotCode,
      drawerCode: 'B',
      status: 'Running'
    }));
    const resolved = steps.map(step => {
      const target = runtimeStepTarget(step);
      return { slotCode: step.slotCode, name: target?.name, x: target?.x, y: target?.y };
    });

    const firstSlide = findTarget('R21');
    const visited = [];
    for(const [index, step] of steps.entries()) {
      applyRuntimeDrivenState({
        runId: 'timeline-browser-acceptance',
        status: 'Running',
        completedStepCount: index,
        totalStepCount: steps.length,
        currentStep: step,
        steps
      }, {
        // Deliberately stale telemetry for the first slide. The active Timeline
        // target must win unless telemetry is correlated to the same step id.
        x: firstSlide.x,
        y: firstSlide.y,
        z1: 0.65,
        z2: 0.35,
        workflowStepExecutionId: 'stale-step'
      });
      await new Promise(resolve => setTimeout(resolve, 1900));
      visited.push({ slotCode: step.slotCode, x: arm.x, y: arm.y, target: currentTarget?.name });
    }

    // Backend Home is numeric (0,0,0), while the SVG home marker is the wash-inner station.
    // Verify a completed/non-running snapshot still animates to that visible home marker.
    applyRuntimeDrivenState({
      runId: 'timeline-browser-acceptance',
      status: 'Completed',
      completedStepCount: steps.length,
      totalStepCount: steps.length,
      currentStep: null,
      steps
    }, {
      x: 0,
      y: 0,
      z1: 0,
      z2: 0,
      targetPointCode: 'Home',
      workflowStepExecutionId: null
    });
    await new Promise(resolve => setTimeout(resolve, 1900));
    const home = getHomePosition();
    const homeVisit = { x: arm.x, y: arm.y, target: currentTarget?.name, expectedX: home.x, expectedY: home.y };
    return { resolved, visited, homeVisit };
  });

  assert(result.resolved.map(x => x.name).join(',') === 'R21,R22,R23',
    `Timeline target resolution was wrong: ${JSON.stringify(result.resolved)}`);
  assert(new Set(result.resolved.map(x => `${x.x},${x.y}`)).size === 3,
    `Timeline targets were not distinct: ${JSON.stringify(result.resolved)}`);
  for(const [index, visit] of result.visited.entries()) {
    const target = result.resolved[index];
    assert(visit.target === target.name && Math.abs(visit.x - target.x) < 0.05 && Math.abs(visit.y - target.y) < 0.05,
      `Arm did not reach Timeline target ${target.name}: ${JSON.stringify(visit)}`);
  }
  assert(result.homeVisit.target === '洗针头_右列_洗内壁_R1'
      && Math.abs(result.homeVisit.x - result.homeVisit.expectedX) < 0.05
      && Math.abs(result.homeVisit.y - result.homeVisit.expectedY) < 0.05,
    `Arm did not reach the visual Home target: ${JSON.stringify(result.homeVisit)}`);
  assert(pageErrors.length === 0, `Page errors occurred: ${JSON.stringify(pageErrors)}`);
  console.log(`[timeline-browser] PASS targets=${result.visited.map(x => x.target).join(' -> ')} -> Home base=${baseURL}`);
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
