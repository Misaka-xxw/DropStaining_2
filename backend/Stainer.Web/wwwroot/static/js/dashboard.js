async function initializeSystem(){
  try{
    const latest = await api('/api/device-initialization');
    let result;
    if(latest?.runId && latest.status === 'Failed'){
      const reason = window.prompt('请输入重试初始化原因：', '设备检查失败后重试');
      if(!reason?.trim()) return;
      result = await api(`/api/device-initialization/${encodeURIComponent(latest.runId)}/retry`, {
        method:'POST',
        body: JSON.stringify({commandId: commandId('device-initialization-retry'), reason: reason.trim()})
      });
    }else{
      result = await api('/api/device-initialization', {
        method:'POST',
        body: JSON.stringify({commandId: commandId('device-initialization')})
      });
    }
    toast(result.message || (result.ok ? '设备初始化完成' : '设备初始化失败'), !result.ok);
    setTimeout(()=>location.reload(), 350);
  }catch(e){
    toast(e.message || '设备初始化失败', true);
  }
}
