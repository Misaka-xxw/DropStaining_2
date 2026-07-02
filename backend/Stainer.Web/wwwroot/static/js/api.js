async function api(url, options={}){
  const request = Object.assign({}, options);
  request.headers = Object.assign({'Content-Type':'application/json'}, options.headers || {});
  const method = String(request.method || 'GET').toUpperCase();
  const active = options.busyElement || (method !== 'GET' && document.activeElement instanceof HTMLButtonElement ? document.activeElement : null);
  delete request.busyElement;
  const priorDisabled = active?.disabled;
  if(active){
    active.disabled = true;
    active.setAttribute('aria-busy', 'true');
  }
  try{
    const res = await fetch(url, request);
    let data = null;
    const text = await res.text();
    try { data = text ? JSON.parse(text) : {}; } catch(e) { data = {raw:text}; }
    if(!res.ok){
      const msg = data.detail || data.message || ('请求失败：' + res.status);
      toast(msg, true);
      const error = new Error(msg);
      error.status = res.status;
      error.data = data;
      throw error;
    }
    return data;
  }finally{
    if(active){
      active.disabled = !!priorDisabled;
      active.removeAttribute('aria-busy');
    }
  }
}
function setButtonDisabledReason(button, disabled, reason=''){
  if(!button) return;
  button.disabled = !!disabled;
  button.title = disabled ? reason : '';
  button.setAttribute('aria-disabled', disabled ? 'true' : 'false');
}
function toast(message, danger=false){
  const el = document.getElementById('toast');
  if(!el) return alert(message);
  el.textContent = message;
  el.classList.remove('hidden');
  el.style.background = danger ? '#991b1b' : '#07111f';
  setTimeout(()=> el.classList.add('hidden'), 2800);
}
async function logout(){
  try{ await api('/api/logout', {method:'POST'}); }catch(e){}
  location.href='/';
}
function initializeUserMenu(){
  const card = document.getElementById('operatorCard');
  const menu = document.getElementById('userMenu');
  const logoutButton = document.getElementById('logoutButton');
  if(!card || !menu || !logoutButton) return;

  const setOpen = open => {
    menu.classList.toggle('hidden', !open);
    card.setAttribute('aria-expanded', open ? 'true' : 'false');
  };
  const toggle = event => {
    event.preventDefault();
    event.stopPropagation();
    setOpen(menu.classList.contains('hidden'));
  };

  card.addEventListener('click', event => {
    if(event.target instanceof Element && event.target.closest('#logoutButton')) return;
    toggle(event);
  });
  card.addEventListener('keydown', event => {
    if(event.key === 'Enter' || event.key === ' ') toggle(event);
  });
  logoutButton.addEventListener('click', async event => {
    event.preventDefault();
    event.stopPropagation();
    logoutButton.disabled = true;
    await logout();
  });
  document.addEventListener('click', () => setOpen(false));
  document.addEventListener('keydown', event => {
    if(event.key === 'Escape') setOpen(false);
  });
}
function statusText(s){
  const map = {idle:'空闲',initialized:'已初始化',ready:'就绪',running:'运行中',paused:'暂停',stopped:'已停止/待处理',completed:'完成',error:'故障',unknown:'Unknown / 待人工处理',empty:'可上样',loaded:'待确认',configured:'待启动',waiting:'等待/待卸载',dispensing:'加液',incubating:'孵育',washing:'通道清洗',mixing:'通道混匀'};
  return map[s] || s;
}

function syncStatusLabels(root=document){
  root.querySelectorAll('[data-status-label]').forEach(el=>{ el.textContent=statusText(el.textContent.trim()); });
}
function updateClock(){
  const t = document.getElementById('clockTime');
  const d = document.getElementById('clockDate');
  if(!t || !d) return;
  const now = new Date();
  t.textContent = now.toLocaleTimeString('zh-CN',{hour12:false,hour:'2-digit',minute:'2-digit'});
  d.textContent = now.toLocaleDateString('zh-CN',{month:'2-digit',day:'2-digit',weekday:'short'});
}
function markActiveNav(){
  const path = location.pathname || '/control-console';
  document.querySelectorAll('.nav-item').forEach(a=>a.classList.toggle('active', a.dataset.href === path));
}
document.addEventListener('DOMContentLoaded',()=>{ syncStatusLabels(); updateClock(); setInterval(updateClock,1000); markActiveNav(); initializeUserMenu(); });
