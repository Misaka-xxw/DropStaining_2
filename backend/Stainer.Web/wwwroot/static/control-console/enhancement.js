/* global digitalTwin, scanSamples, scanReagents, simulateLowReagent, simulateAlarm, togglePullChannel, runDemo, resetDemo, log, setConn */
(() => {
  'use strict';
  const $ = (sel, root = document) => root.querySelector(sel);
  const $$ = (sel, root = document) => Array.from(root.querySelectorAll(sel));
  const api = {
    ws: null,
    connected: false,
    mode: 'sim',
    role: '实验员',
    flow: 'IHC 快速冰冻染色',
    sampleCount: 16,
    lastEvents: [],
    protocol: [
      { key: 'init', name: '初始化', duration: '设备自检' },
      { key: 'prep', name: '准备检查', duration: '水/废液/排毒' },
      { key: 'sample_scan', name: '样本扫描', duration: '机械臂扫码' },
      { key: 'reagent_scan', name: '试剂扫描', duration: '5×8 试剂位' },
      { key: 'block', name: '阻断剂', duration: '20s 混匀' },
      { key: 'primary', name: '一抗孵育', duration: '4.5min' },
      { key: 'secondary', name: '二抗孵育', duration: '1.5min' },
      { key: 'dab', name: 'DAB 显色', duration: '1.5min' },
      { key: 'hematoxylin', name: '苏木素', duration: '10s' },
      { key: 'final', name: '结束清洗', duration: '10s' }
    ],
    stepKey: 'init'
  };

  function safeLog(msg, type = '') {
    if (typeof log === 'function') log(msg, type);
    console[type === 'err' ? 'error' : 'log'](msg);
  }

  function ensureShell() {
    document.body.classList.add('console-mode');
    addHeaderChips();
    addRibbon();
    addDock();
    addModalShell();
    connectWebSocket();
    renderProtocol();
    safeLog('主控台增强层已加载：弹窗式业务入口 + FastAPI 事件适配器', 'ok');
  }

  function addHeaderChips() {
    const top = $('.top-status');
    if (!top || $('#modeChip')) return;
    const modeChip = document.createElement('span');
    modeChip.className = 'console-chip';
    modeChip.id = 'modeChip';
    modeChip.innerHTML = '<span class="mini-dot"></span>模式：<b>模拟</b>';
    const wsChip = document.createElement('span');
    wsChip.className = 'console-chip';
    wsChip.id = 'wsChip';
    wsChip.innerHTML = '<span class="mini-dot warn"></span>后端：<b>连接中</b>';
    const roleChip = document.createElement('button');
    roleChip.className = 'console-chip';
    roleChip.id = 'roleChip';
    roleChip.innerHTML = '角色：<b>实验员</b>';
    roleChip.addEventListener('click', () => openModal('login'));
    top.prepend(roleChip, wsChip, modeChip);
  }

  function addRibbon() {
    const left = $('.toolbar-left');
    const right = $('.toolbar-right');
    if (!left || $('#consoleRibbon')) return;
    const ribbon = document.createElement('div');
    ribbon.className = 'console-ribbon';
    ribbon.id = 'consoleRibbon';
    ribbon.innerHTML = `
      <button class="major" data-modal="flow">流程</button>
      <button class="safe" data-command="prep_check">准备检查</button>
      <button data-command="scan_samples">样本扫码</button>
      <button data-command="scan_reagents">试剂扫码</button>
      <button class="major" data-command="start_run">开始运行</button>
      <button class="warn" data-modal="addSample">中途加样</button>
      <button class="muted" data-modal="reagent">试剂管理</button>
      <button class="muted" data-modal="engineer">调试</button>
    `;
    ribbon.addEventListener('click', e => {
      const btn = e.target.closest('button');
      if (!btn) return;
      if (btn.dataset.modal) openModal(btn.dataset.modal);
      if (btn.dataset.command) sendCommand(btn.dataset.command);
    });
    left.prepend(ribbon);

    const modeSwitch = document.createElement('div');
    modeSwitch.className = 'mode-switch';
    modeSwitch.innerHTML = '<button data-mode="sim" class="active">模拟</button><button data-mode="real">真实</button>';
    modeSwitch.addEventListener('click', e => {
      const btn = e.target.closest('button[data-mode]');
      if (!btn) return;
      api.mode = btn.dataset.mode;
      $$('.mode-switch button').forEach(b => b.classList.toggle('active', b.dataset.mode === api.mode));
      refreshHeaderChips();
      sendCommand('set_mode', { mode: api.mode });
    });
    right.prepend(modeSwitch);

    const alarmBtn = document.createElement('button');
    alarmBtn.className = 'danger';
    alarmBtn.textContent = '报警处理';
    alarmBtn.addEventListener('click', () => openModal('alarm'));
    right.append(alarmBtn);
  }

  function addDock() {
    const wrap = $('.map-wrap');
    if (!wrap || $('#commandDock')) return;
    const dock = document.createElement('div');
    dock.className = 'command-dock collapsed';
    dock.id = 'commandDock';
    dock.innerHTML = `
      <h3 id="dockHeader"><span class="dock-head-left"><span class="dock-title">流程队列</span><span class="micro" id="dockMode">模拟联机</span></span><span class="dock-caret">▸</span></h3>
      <div class="api-note" id="dockNote">当前页面作为主操作面板：左侧数字孪生负责设备态势，业务功能以弹窗进入，后端通过 WebSocket 推送状态。</div>
      <div class="protocol-steps" id="protocolSteps" style="margin-top:10px"></div>
    `;
    wrap.appendChild(dock);
    const dockHeader = $('#dockHeader');
    if (dockHeader) dockHeader.addEventListener('click', () => {
      const collapsed = dock.classList.toggle('collapsed');
      const caret = dock.querySelector('.dock-caret');
      if (caret) caret.textContent = collapsed ? '▸' : '▾';
    });
  }

  function addModalShell() {
    if ($('#consoleModalBackdrop')) return;
    const modal = document.createElement('div');
    modal.className = 'console-modal-backdrop';
    modal.id = 'consoleModalBackdrop';
    modal.innerHTML = `
      <section class="console-modal" role="dialog" aria-modal="true" aria-labelledby="consoleModalTitle">
        <header>
          <div><h2 id="consoleModalTitle">弹窗</h2><p id="consoleModalSub">—</p></div>
          <button class="close-x" id="consoleModalClose">关闭</button>
        </header>
        <div class="body" id="consoleModalBody"></div>
        <footer id="consoleModalFooter"></footer>
      </section>`;
    document.body.appendChild(modal);
    $('#consoleModalClose').addEventListener('click', closeModal);
    modal.addEventListener('click', e => { if (e.target === modal) closeModal(); });
  }

  function openModal(kind) {
    const meta = getModalContent(kind);
    $('#consoleModalTitle').textContent = meta.title;
    $('#consoleModalSub').textContent = meta.sub;
    $('#consoleModalBody').innerHTML = meta.body;
    $('#consoleModalFooter').innerHTML = meta.footer;
    bindModal(kind);
    $('#consoleModalBackdrop').classList.add('show');
  }

  function closeModal() { $('#consoleModalBackdrop').classList.remove('show'); }

  function getModalContent(kind) {
    const footClose = '<button id="modalCancel">关闭</button>';
    if (kind === 'login') return {
      title: '登录 / 角色切换', sub: '角色决定可见功能；登录成功后触发仪器初始化。',
      body: `
        <div class="form-grid">
          <div class="form-card">
            <h3>身份信息</h3>
            <label>用户名<input id="loginUser" value="operator" autocomplete="username"></label>
            <label>密码<input id="loginPass" type="password" value="123456" autocomplete="current-password"></label>
            <div class="role-pills" id="rolePills">
              <div class="role-pill active" data-role="实验员"><strong>实验员</strong><p>运行、暂停、加样、报警确认</p></div>
              <div class="role-pill" data-role="工程师"><strong>工程师</strong><p>调试、坐标、COM、模块测试</p></div>
              <div class="role-pill" data-role="管理员"><strong>管理员</strong><p>用户、权限、审计记录</p></div>
            </div>
          </div>
          <div class="form-card">
            <h3>登录后初始化</h3>
            <p>机械臂回原点、打开制冷、验证扫码头通信、查询液位传感器、执行洗针。异常时进入报警处理弹窗。</p>
            <div class="check-list">
              <div class="check-row"><span>机械臂</span><b class="badge info">待初始化</b></div>
              <div class="check-row"><span>制冷模块</span><b class="badge info">待开启</b></div>
              <div class="check-row"><span>扫码头</span><b class="badge info">待通信</b></div>
              <div class="check-row"><span>液位传感器</span><b class="badge info">待查询</b></div>
            </div>
          </div>
        </div>`,
      footer: `${footClose}<button class="primary" id="loginConfirm">登录并初始化</button>`
    };
    if (kind === 'flow') return {
      title: '流程选择', sub: '新建流程、恢复中断流程，或选择实验脚本。',
      body: `
        <div class="form-grid">
          <div class="form-card">
            <h3>流程入口</h3>
            <div class="flow-cards" id="flowCards">
              <div class="flow-card active" data-flow="IHC 快速冰冻染色"><strong>IHC 快速冰冻染色</strong><small>阻断 → 一抗 → 二抗 → DAB → 苏木素</small></div>
              <div class="flow-card" data-flow="HE 快速染色"><strong>HE 快速染色</strong><small>用于 HE 脚本，适合快速常规染色。</small></div>
              <div class="flow-card" data-flow="恢复中断流程"><strong>恢复中断流程</strong><small>从最近完成的大步骤恢复，保留玻片/试剂映射。</small></div>
            </div>
          </div>
          <div class="form-card">
            <h3>批次参数</h3>
            <label>单次上样数量<input id="flowSamples" type="number" min="1" max="16" value="16"></label>
            <label>LIS 连接<select id="flowLis"><option>暂不连接</option><option>启用 LIS 查询</option></select></label>
            <label>实验流程文件<select id="flowFile"><option>内置 IHC_Quick.yaml</option><option>内置 HE_Quick.yaml</option><option>选择本地流程文件...</option></select></label>
            <p>DAB 配制量将按样本数自动计算：每片 0.2 ml，并额外多配 0.4 ml。</p>
          </div>
        </div>`,
      footer: `${footClose}<button id="flowPrep">确认并进入准备检查</button><button class="primary" id="flowConfirm">确认流程</button>`
    };
    if (kind === 'addSample') return {
      title: '中途添加样本', sub: '空闲或已完成通道可抽出加样，不影响其他通道运行。',
      body: `
        <div class="form-grid three">
          <div class="form-card"><h3>上样位置</h3><label>通道<select id="addChannel"><option>通道1</option><option>通道2</option><option>通道3</option><option>通道4</option></select></label><label>新增玻片数<input id="addCount" type="number" min="1" max="4" value="2"></label></div>
          <div class="form-card"><h3>脚本</h3><label>流程<select id="addScript"><option>IHC</option><option>HE</option><option>PAS</option><option>MAS</option><option>IF</option></select></label><label>一抗代码<input id="addAntibody" value="Ab-CK7"></label></div>
          <div class="form-card"><h3>执行策略</h3><p>加入队列后，调度器按通道独立状态穿插执行。若试剂缺失，先弹出试剂补充提示。</p><div class="check-row"><span>通道门状态</span><b class="badge ok">允许抽出</b></div></div>
        </div>`,
      footer: `${footClose}<button id="addPull">模拟抽出通道</button><button class="primary" id="addConfirm">加入运行队列</button>`
    };
    if (kind === 'reagent') return {
      title: '试剂管理 / DAB 配制', sub: '5 列×8 位试剂架，显示映射、余量、阈值和临时配液。',
      body: `<div class="form-grid"><div class="form-card"><h3>试剂位映射</h3>${reagentGridHtml()}</div><div class="form-card"><h3>关键试剂</h3><div class="check-list">
        <div class="check-row"><span>内源性酶阻断剂</span><b class="badge ok">足量</b></div>
        <div class="check-row"><span>一抗 Ab-CK7</span><b class="badge warn">低余量</b></div>
        <div class="check-row"><span>二抗</span><b class="badge ok">双套上载</b></div>
        <div class="check-row"><span>DAB A/B/纯水</span><b class="badge info" id="dabCalc">${dabVolumeText()}</b></div>
        <div class="check-row"><span>清洗液</span><b class="badge ok">泵控</b></div>
      </div><label>最低报警阈值 %<input id="reagentThreshold" type="number" min="1" max="50" value="15"></label><p>绿色为流程需要试剂，橙色为低余量示例；真实模式由后端根据条码和液位传感器刷新。</p></div></div>`,
      footer: `${footClose}<button id="reagentScan">重新扫描试剂区</button><button class="danger" id="reagentLow">模拟试剂不足</button>`
    };
    if (kind === 'alarm') return {
      title: '报警处理', sub: '液位、试剂、通道抽出和故障恢复集中处理。',
      body: `<div class="form-grid"><div class="form-card"><h3>当前报警</h3><div class="alarm-list">
        <div class="alarm-row"><span>纯水桶液位</span><b class="badge err">不足</b></div>
        <div class="alarm-row"><span>废液桶</span><b class="badge warn">接近满</b></div>
        <div class="alarm-row"><span>排毒桶</span><b class="badge warn">接近满</b></div>
        <div class="alarm-row"><span>通道抽出</span><b class="badge info">可独立恢复</b></div>
      </div></div><div class="form-card"><h3>恢复策略</h3><p>严重报警需人工确认后继续；运行故障支持从最近完成的大步骤重新开始。</p><label>恢复点<select id="recoverStep"><option>准备检查</option><option>样本扫描完成</option><option>试剂扫描完成</option><option>一抗孵育后</option><option>DAB 显色后</option></select></label><textarea id="alarmRemark">已处理液位和耗材。</textarea></div></div>`,
      footer: `${footClose}<button id="alarmSim">模拟液位报警</button><button id="recoverRun">从大步骤恢复</button><button class="primary" id="alarmConfirm">确认继续</button>`
    };
    if (kind === 'engineer') return {
      title: '工程师调试', sub: 'COM、扫码器、移液、混匀、清洗模块测试入口。',
      body: `<div class="form-grid"><div class="form-card"><h3>通讯参数</h3><label>COM 口<input value="COM3"></label><label>波特率<input value="115200"></label><label>数据位 / 停止位 / 校验<input value="8 bits / 1 bit / None"></label><div class="api-note">真实设备调试命令只发送到后端，前端不直接操作运动控制或泵。</div></div><div class="form-card"><h3>单模块测试</h3><div class="check-list"><button data-test="arm_home">机械臂回原点</button><button data-test="barcode">扫码器测试</button><button data-test="pipette">通道移液测试</button><button data-test="mix">混匀电机测试</button><button data-test="wash">洗针/清洗测试</button></div></div></div><div class="form-card" style="margin-top:12px"><h3>后端事件</h3><div class="timeline" id="modalTimeline">${eventTimelineHtml()}</div></div>`,
      footer: `${footClose}<button id="engineerSave">保存调试参数</button>`
    };
    return { title: '信息', sub: '', body: '<p>未定义弹窗。</p>', footer: footClose };
  }

  function bindModal(kind) {
    $('#modalCancel')?.addEventListener('click', closeModal);
    if (kind === 'login') {
      $$('#rolePills .role-pill').forEach(el => el.addEventListener('click', () => {
        $$('#rolePills .role-pill').forEach(x => x.classList.remove('active'));
        el.classList.add('active'); api.role = el.dataset.role; refreshHeaderChips();
      }));
      $('#loginConfirm')?.addEventListener('click', () => { sendCommand('login_init', { role: api.role, user: $('#loginUser').value }); closeModal(); });
    }
    if (kind === 'flow') {
      $$('#flowCards .flow-card').forEach(el => el.addEventListener('click', () => {
        $$('#flowCards .flow-card').forEach(x => x.classList.remove('active'));
        el.classList.add('active'); api.flow = el.dataset.flow;
      }));
      $('#flowConfirm')?.addEventListener('click', () => {
        api.sampleCount = Number($('#flowSamples').value || 16);
        sendCommand('select_flow', { flow: api.flow, samples: api.sampleCount, lis: $('#flowLis').value });
        closeModal();
      });
      $('#flowPrep')?.addEventListener('click', () => { sendCommand('select_flow', { flow: api.flow, samples: Number($('#flowSamples').value || 16) }); sendCommand('prep_check'); closeModal(); });
    }
    if (kind === 'addSample') {
      $('#addPull')?.addEventListener('click', () => sendCommand('pull_channel', { channel: Number($('#addChannel').value.replace(/\D/g, '')) || 1 }));
      $('#addConfirm')?.addEventListener('click', () => {
        sendCommand('add_sample', { channel: Number($('#addChannel').value.replace(/\D/g, '')) || 1, count: Number($('#addCount').value || 1), script: $('#addScript').value, antibody: $('#addAntibody').value });
        closeModal();
      });
    }
    if (kind === 'reagent') {
      $('#reagentScan')?.addEventListener('click', () => { sendCommand('scan_reagents'); closeModal(); });
      $('#reagentLow')?.addEventListener('click', () => sendCommand('low_reagent'));
    }
    if (kind === 'alarm') {
      $('#alarmSim')?.addEventListener('click', () => sendCommand('liquid_alarm'));
      $('#recoverRun')?.addEventListener('click', () => sendCommand('recover_step', { step: $('#recoverStep').value, remark: $('#alarmRemark').value }));
      $('#alarmConfirm')?.addEventListener('click', () => { sendCommand('alarm_confirm', { remark: $('#alarmRemark').value }); closeModal(); });
    }
    if (kind === 'engineer') {
      $$('#consoleModalBody button[data-test]').forEach(btn => btn.addEventListener('click', () => sendCommand('engineer_test', { test: btn.dataset.test })));
      $('#engineerSave')?.addEventListener('click', () => { sendCommand('save_engineer_config'); closeModal(); });
    }
  }

  function reagentGridHtml() {
    let html = '<div class="reagent-grid">';
    for (let col = 1; col <= 5; col++) {
      html += `<div class="reagent-rack"><b>试剂架 ${col}</b>`;
      for (let row = 1; row <= 8; row++) {
        const idx = (col - 1) * 8 + row;
        const cls = [1, 9, 17, 25, 33].includes(idx) ? ' req' : [12, 27].includes(idx) ? ' low' : '';
        html += `<div class="reagent-cell${cls}">R${idx}</div>`;
      }
      html += '</div>';
    }
    html += '</div>';
    return html;
  }

  function dabVolumeText() {
    const total = api.sampleCount * 0.2 + 0.4;
    const a = total / 20;
    const b = total / 20;
    const water = total * 18 / 20;
    return `${total.toFixed(1)}ml：A ${a.toFixed(2)} / B ${b.toFixed(2)} / 水 ${water.toFixed(2)}`;
  }

  function eventTimelineHtml() {
    return api.lastEvents.slice(0, 15).map(e => `<p>[${e.time}] ${escapeHtml(e.message || e.type || 'event')}</p>`).join('') || '<p>暂无后端事件。</p>';
  }

  function escapeHtml(s) { return String(s).replace(/[&<>"]/g, c => ({ '&': '&amp;', '<': '&lt;', '>': '&gt;', '"': '&quot;' }[c])); }

  async function sendCommand(command, payload = {}) {
    const body = { command, mode: api.mode, payload };
    try {
      const res = await fetch('/api/command', { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(body) });
      if (!res.ok) throw new Error(`HTTP ${res.status}`);
      const event = await res.json();
      applyBackendEvent(event);
      return event;
    } catch (err) {
      fallbackCommand(command, payload);
      return null;
    }
  }

  function fallbackCommand(command, payload = {}) {
    safeLog(`后端不可用，使用前端本地模拟：${command}`, 'warn');
    if (command === 'login_init') { setConn?.('ok'); safeLog(`用户 ${payload.user || 'operator'} 以${api.role}登录，初始化完成`, 'ok'); }
    if (command === 'select_flow') { api.flow = payload.flow || api.flow; api.sampleCount = payload.samples || api.sampleCount; $('#phaseText') && ($('#phaseText').textContent = '流程已选择'); safeLog(`已选择流程：${api.flow}`, 'ok'); }
    if (command === 'prep_check') { digitalTwin?.update?.({ liquids: { pure: 82, pbs: 76, waste: 18, toxic: 12 } }); safeLog('准备检查通过：水箱/废液/排毒桶状态正常', 'ok'); }
    if (command === 'scan_samples') scanSamples?.();
    if (command === 'scan_reagents') scanReagents?.();
    if (command === 'start_run') runDemo?.();
    if (command === 'low_reagent') simulateLowReagent?.();
    if (command === 'liquid_alarm') simulateAlarm?.();
    if (command === 'pull_channel') togglePullChannel?.();
    if (command === 'add_sample') safeLog(`中途加样：通道${payload.channel} 新增 ${payload.count} 张，脚本 ${payload.script}`, 'ok');
    if (command === 'recover_step') safeLog(`从大步骤恢复：${payload.step}`, 'ok');
    if (command === 'engineer_test') safeLog(`工程师测试命令已发出：${payload.test}`, 'warn');
  }

  function connectWebSocket() {
    const proto = location.protocol === 'https:' ? 'wss:' : 'ws:';
    try {
      const ws = new WebSocket(`${proto}//${location.host}/ws/events`);
      api.ws = ws;
      ws.onopen = () => { api.connected = true; refreshHeaderChips(); safeLog('FastAPI WebSocket 已连接', 'ok'); };
      ws.onclose = () => { api.connected = false; refreshHeaderChips(); setTimeout(connectWebSocket, 2500); };
      ws.onerror = () => { api.connected = false; refreshHeaderChips(); };
      ws.onmessage = ev => {
        try { applyBackendEvent(JSON.parse(ev.data)); }
        catch (err) { console.warn(err); }
      };
    } catch (err) {
      api.connected = false;
      refreshHeaderChips();
    }
  }

  function applyBackendEvent(event = {}) {
    const now = new Date().toLocaleTimeString('zh-CN', { hour12: false });
    if (event.message) api.lastEvents.unshift({ time: now, message: event.message, type: event.type });
    api.lastEvents = api.lastEvents.slice(0, 80);
    if (event.payload) digitalTwin?.update?.(event.payload);
    if (event.meta) applyMeta(event.meta);
    if (event.message) safeLog(event.message, event.severity || '');
    if (event.step_key) { api.stepKey = event.step_key; renderProtocol(); }
    refreshHeaderChips();
    const tl = $('#modalTimeline'); if (tl) tl.innerHTML = eventTimelineHtml();
  }

  function applyMeta(meta = {}) {
    if (meta.phase && $('#phaseText')) $('#phaseText').textContent = meta.phase;
    if (Number.isFinite(meta.sample_count) && $('#sampleCount')) $('#sampleCount').textContent = meta.sample_count;
    if (Number.isFinite(meta.progress)) {
      if ($('#overallBar')) $('#overallBar').style.width = `${Math.max(0, Math.min(100, meta.progress))}%`;
      if ($('#kpiProgress')) $('#kpiProgress').textContent = `${Math.round(meta.progress)}%`;
    }
    if (meta.step_text && $('#kpiStep')) $('#kpiStep').textContent = meta.step_text;
    if (Number.isFinite(meta.temperature) && $('#kpiTemp')) $('#kpiTemp').textContent = `${meta.temperature.toFixed(1)}℃`;
  }

  function refreshHeaderChips() {
    const modeChip = $('#modeChip');
    if (modeChip) {
      modeChip.querySelector('b').textContent = api.mode === 'real' ? '真实' : '模拟';
      modeChip.querySelector('.mini-dot').className = `mini-dot${api.mode === 'real' ? ' warn' : ''}`;
    }
    const wsChip = $('#wsChip');
    if (wsChip) {
      wsChip.querySelector('b').textContent = api.connected ? '已连接' : '离线/本地';
      wsChip.querySelector('.mini-dot').className = `mini-dot${api.connected ? '' : ' warn'}`;
    }
    const roleChip = $('#roleChip');
    if (roleChip) roleChip.querySelector('b').textContent = api.role;
    const dockMode = $('#dockMode');
    if (dockMode) dockMode.textContent = `${api.mode === 'real' ? '真实设备' : '模拟联机'} / ${api.connected ? 'WS' : 'local'}`;
  }

  function renderProtocol() {
    const box = $('#protocolSteps');
    if (!box) return;
    const activeIndex = Math.max(0, api.protocol.findIndex(s => s.key === api.stepKey));
    box.innerHTML = api.protocol.map((s, i) => {
      const cls = i < activeIndex ? 'done' : i === activeIndex ? 'active' : '';
      return `<div class="protocol-step ${cls}"><i>${i < activeIndex ? '✓' : i + 1}</i><div><b>${s.name}</b><br><span>${s.duration}</span></div><span>${cls === 'active' ? '执行中' : cls === 'done' ? '完成' : '等待'}</span></div>`;
    }).join('');
  }

  if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', ensureShell);
  else ensureShell();

  window.stainerConsole = { sendCommand, openModal, closeModal, api };
})();
