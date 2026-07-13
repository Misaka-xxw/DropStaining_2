# UI 控件 → 后端 API 映射表

> 生成日期：2026-07-13。权威源：线上 UI 由 [`LegacyUiPageRenderer.cs`](../backend/Stainer.Web/Infrastructure/Web/LegacyUiPageRenderer.cs) 服务端内联渲染 + [`wwwroot/static/js/`](../backend/Stainer.Web/wwwroot/static/js/) 提供行为；后端为 ASP.NET minimal API（[`Infrastructure/Web/WebHostEndpointExtensions.*.cs`](../backend/Stainer.Web/Infrastructure/Web/)）。
>
> **不在范围内**：`src/app/templates/*.html`（旧 Python Flask 模板，已被 .NET 渲染器取代，含 `testPipette`/`engineerCommand` 等已废弃控件）；`wwwroot/twin/index.html`（747KB 数字孪生打包页，独立控制面，仅消费 `/api/login`、`/api/users`、`/api/roles`、`/api/twin/snapshot`，其余多为客户端 Mock）。

## 0. 架构与鉴权模型

- **身份机制**：Cookie 会话 `stainer_session`（HttpOnly / SameSite=Strict），由 `UserSessionService` 内存字典维护；登录后选角色写入 cookie。
- **鉴权方式**：**没有** ASP.NET `AddAuthorization`/策略；每个端点在 handler 内命令式调用 `RequireAnyRoleAsync(ctx, roles[])` 或 `RequireRoleAsync`。工程写操作额外要求 `engineeringSessionService.RequireWriteSessionAsync`（二次认证会话）。
- **角色**：`admin` / `engineer` / `operator`（登录时单选一个 ActiveRole）。
- **请求体约定**：所有现代写端点 body 含 `commandId`（前端 `commandId(prefix)` 生成 UUID）。C# record 默认序列化为 camelCase JSON。
- **前端统一 helper**：`api(url, options)`（[api.js:1](../backend/Stainer.Web/wwwroot/static/js/api.js#L1)）封装 fetch + JSON + 错误 toast + 按钮 busy 态。`run.js` 另有 `fetchJsonOrNull`（404 容忍），建议合并。
- **实时推送**：SignalR `/hubs/machine`（[stainer-host.js:670](../backend/Stainer.Web/wwwroot/static/js/stainer-host.js#L670)），仅本地请求 + 有效会话。事件触发 180ms 防抖后 `GET /api/operator/snapshot` 刷新。

页面脚本加载（[LegacyUiPageRenderer.cs RenderShell](../backend/Stainer.Web/Infrastructure/Web/LegacyUiPageRenderer.cs#L78)）：全局加载 `api.js` + `stainer-host.js`；`dashboard`/`run`/`engineer`/`mock-timeline` 各自追加页脚本。**`configure.js` 不被任何线上页面加载** → 其 `saveSlide()`→`POST /api/slides/configure` 是死代码。

---

## 1. 逐页映射表

列：控件 | 处理函数(文件:行) | Method + Path | 请求体 | 所需角色

### 1.1 Shell（所有页面）
| 控件 | 处理函数 | API | Body | 角色 |
|---|---|---|---|---|
| 退出登录 `#logoutButton` | `logout()` [api.js:129](../backend/Stainer.Web/wwwroot/static/js/api.js#L129) | `POST /api/logout` | — | 匿名（内部校验 cookie） |
| 导航项 / 品牌卡 / 告警条 | `location.href=...` | — | — | 纯导航 |
| 时钟 / 状态文案 / Toast | `updateClock`/`syncStatusLabels`/`toast` | — | — | 纯客户端 |

### 1.2 `/` 登录页
| 控件 | 处理函数 | API | Body | 角色 |
|---|---|---|---|---|
| 账号 `#username` / 密码 `#password` | `login(role)` [LegacyUiPageRenderer.cs:196](../backend/Stainer.Web/Infrastructure/Web/LegacyUiPageRenderer.cs#L196) | （拼装 payload） | — | — |
| 管理员登录 / 实验员登录 | `login('admin'\|'operator')` | `POST /api/login` | `{username,password,role}` | 匿名 |
| 回车提交 | 同上 | `POST /api/login` | 同上 | 匿名 |

### 1.3 `/dashboard` 检查
| 控件 | 处理函数 | API | Body | 角色 |
|---|---|---|---|---|
| 执行 / 重试初始化 | `initializeSystem()` [dashboard.js:1](../backend/Stainer.Web/wwwroot/static/js/dashboard.js#L1) | `GET /api/device-initialization` → `POST /api/device-initialization`（首次）或 `POST /api/device-initialization/{runId}/retry`（重试） | 首次 `{commandId}`；重试 `{commandId, reason}` | operator/engineer/admin |
| 查看事件 | `openDashboardEventList()` [stainer-host.js:94](../backend/Stainer.Web/wwwroot/static/js/stainer-host.js#L94) | — | — | 读缓存事件，无直接 fetch |
| 页面载入 | `loadHostState` | `GET /api/operator/snapshot` | — | operator/engineer/admin |
| 抽屉「通道详情 / 查看告警」 | `location.href` | — | — | 纯导航 |
| 告警条「查看处理」按钮 | 无 onclick | — | — | ⚠ 装饰性，未接线 |

### 1.4 `/samples` 样本
| 控件 | 处理函数 | API | Body | 角色 |
|---|---|---|---|---|
| 刷新 | `scanSamples()` [stainer-host.js:1108](../backend/Stainer.Web/wwwroot/static/js/stainer-host.js#L1108) | `GET /api/operator/snapshot` | — | operator/engineer/admin（注：旧 `POST /api/samples/scan?count=` 已不用） |
| 通灵码 / 医院码-LIS / HE | `openConfirmModal('ihc-tl'\|'ihc-hospital'\|'he')` | — | — | 仅开弹窗 |
| 确认创建任务 | `confirmTask()` [stainer-host.js:1274](../backend/Stainer.Web/wwwroot/static/js/stainer-host.js#L1274) | HE: `POST /api/tasks/he`；IHC: `POST /api/lis/mock-query` 后 `POST /api/tasks/ihc` | HE `{commandId, slotCode, drawerCode?, channelBatchId?, workflowVersionId?}`；IHC `{commandId, inputMode, rawCode, slotCode, drawerCode?, channelBatchId?, selectedPrimaryAntibodyCode?, lisQueryLogId?}` | operator/admin |
| 通道「选择实验类型」 | `openChannelScriptModal(letter,'select')` | `POST /api/channel-batches/active` | `{commandId, drawerCode}` | operator/admin |
| 通道「更换实验类型」 | `openChannelScriptModal(letter,'change')` | `POST /api/channel-batches/active` 后 `POST /api/channel-batches/experiment-type-selection` | `{commandId, channelBatchId?, drawerCode?, experimentType, reason?}` | ⚠ operator/engineer/admin（更宽） |
| 弹窗内 select/input | — | — | — | 客户端态 |

### 1.5 `/reagents` 试剂
| 控件 | 处理函数 | API | Body | 角色 |
|---|---|---|---|---|
| 开始扫码 | `startReagentScanSession()` [stainer-host.js:711](../backend/Stainer.Web/wwwroot/static/js/stainer-host.js#L711) | `POST /api/reagents/scan-sessions/start` | `{commandId}` | operator/admin |
| 完成扫码 | `completeReagentScanSession()` [stainer-host.js:724](../backend/Stainer.Web/wwwroot/static/js/stainer-host.js#L724) | `POST /api/reagents/scan-sessions/{scanSessionId}/complete` | `{commandId}` | operator/admin |
| 扫描全部 | `scanReagents()` [stainer-host.js:750](../backend/Stainer.Web/wwwroot/static/js/stainer-host.js#L750) | `GET /api/reagents/scan-sessions/overview` + `GET /api/reagents/rack` | — | ⚠ 当前匿名 |
| ch1–ch5 按列扫码 | `mockColumnScan(n)` | 同上，按列过滤 | — | 同上 |
| 确认扫码 | `confirmReagentPositionScan()` [stainer-host.js:845](../backend/Stainer.Web/wwwroot/static/js/stainer-host.js#L845) | `POST /api/reagents/scan-confirm` | `{commandId, scanSessionId?, items:[{position, scanResult, rawBarcode?, locatorCode?, expirationDate?}]}` | operator/admin |
| R1–R40 试剂瓶 / DAB M1–M8 / 刷新 | `showReagentDetail`/`showDabPosition`/`refreshDabPositions` | — | — | 读缓存 |
| DAB 启动清洗 | `runDabCleaning(batchId,'start')` [stainer-host.js:442](../backend/Stainer.Web/wwwroot/static/js/stainer-host.js#L442) | `POST /api/dab/batches/{batchId}/cleaning/start` | `{commandId}` | engineer/admin |
| DAB 确认清洗完成 | `runDabCleaning(batchId,'confirm')` | `POST /api/dab/batches/{batchId}/cleaning/confirm` | `{commandId}` | engineer/admin |

### 1.6 `/run` 运行
| 控件 | 处理函数 | API | Body | 角色 |
|---|---|---|---|---|
| 启动前预检 / 启动 | `runAction('start')` [run.js:37](../backend/Stainer.Web/wwwroot/static/js/run.js#L37) | `GET /api/run/preflight` → `POST /api/runs`（建批）→ `POST /api/runs/{id}/start` | 建 `{commandId, stainingTaskIds, preflightStateHash?}`；启 `{commandId, preflightStateHash?}` | operator/admin |
| 暂停 | `runAction('pause')` | `POST /api/runs/{id}/pause` | `{commandId, preflightStateHash?}` | operator/admin |
| 恢复 | `runAction('resume')` | `POST /api/runs/{id}/resume` | `{commandId, preflightStateHash?}` | operator/admin |
| 整机停止 | `confirmStop()`→`runAction('stop')` | `POST /api/runs/{id}/stop` | `{commandId, preflightStateHash?}` | operator/admin |
| Mock 故障 | `injectMockFault()` [run.js:223](../backend/Stainer.Web/wwwroot/static/js/run.js#L223) | `POST /api/runs/{id}/fault` | `{commandId, message}` | engineer/admin |
| 大步骤重做 | `redoCurrentMajorStep()` [run.js:246](../backend/Stainer.Web/wwwroot/static/js/run.js#L246) | `POST /api/runs/{id}/redo-current-major-step` | `{commandId, reason}` | engineer/admin |
| 刷新 | `refreshRun()` [run.js:312](../backend/Stainer.Web/wwwroot/static/js/run.js#L312) | `GET /api/runs/current` + `GET /api/operator/snapshot` + `GET /api/system/info` | — | 读 |
| 查看校验详情 | `openValidationModal()` [run.js:75](../backend/Stainer.Web/wwwroot/static/js/run.js#L75) | `GET /api/run/preflight` | — | operator/engineer/admin |
| 全部通过后启动 | `forceStartAfterValidation()` [run.js:160](../backend/Stainer.Web/wwwroot/static/js/run.js#L160) | `GET /api/run/preflight` → `POST /api/runs/{id}/start` | 同启动 | operator/admin |

### 1.7 `/alerts` 告警
| 控件 | 处理函数 | API | Body | 角色 |
|---|---|---|---|---|
| 查询 | `loadTraceAlarms()` [stainer-host.js:1889](../backend/Stainer.Web/wwwroot/static/js/stainer-host.js#L1889) | `GET /api/alarms?{filters}` | — | operator/engineer/admin |
| 导出告警 CSV | `exportTraceCsv('alarms')` | `GET /api/alarms/export?{filters}` | — | operator/engineer/admin |
| 单条确认 | `acknowledgeTraceAlarm(id)` [stainer-host.js:1913](../backend/Stainer.Web/wwwroot/static/js/stainer-host.js#L1913) | `POST /api/alarms/{alarmId}/acknowledge` | `{commandId, reason?}` | operator/engineer/admin |
| 筛选 select | — | 作为 query 参数 | — | 客户端 |

### 1.8 `/history` 历史与导出
| 控件 | 处理函数 | API | Body | 角色 |
|---|---|---|---|---|
| 查询 | `loadTraceHistory()` | `GET /api/history/runs?{filters}` + `GET /api/history/reagent-consumptions?{filters}` | — | operator/engineer/admin |
| 运行行点击 | `loadTraceRunDetail(id)` | `GET /api/history/runs/{machineRunId}` | — | operator/engineer/admin |
| 导出运行 CSV | `exportTraceCsv('history-runs')` | `GET /api/history/export/runs?{filters}` | — | operator/engineer/admin |
| 导出试剂消耗 CSV | `exportTraceCsv('reagents')` | `GET /api/history/export/reagent-consumptions?{filters}` | — | operator/engineer/admin |
| 刷新审计 | `loadTraceAudit()` | `GET /api/audit/logs?{filters}` | — | ⚠ engineer/admin（operator 见 403） |
| 导出审计 CSV | `exportTraceCsv('audit')` | `GET /api/audit/export?{filters}` | — | ⚠ engineer/admin |
| 11 个筛选 input/select | — | 作为 query 参数 | — | 客户端 |

### 1.9 `/configure` 协议配置
| 控件 | 处理函数 | API | Body | 角色 |
|---|---|---|---|---|
| 新建流程 Draft | `createWorkflowDraft()` [stainer-host.js:1375](../backend/Stainer.Web/wwwroot/static/js/stainer-host.js#L1375) | `POST /api/workflows` | `{commandId, code, name, workflowType, description?, versionLabel?, changeNote?}` | admin |
| 复制为 Draft | `copyWorkflowDraft()` [stainer-host.js:1404](../backend/Stainer.Web/wwwroot/static/js/stainer-host.js#L1404) | `POST /api/workflow-versions/{sourceId}/copy-draft` | `{commandId, versionLabel?, changeNote?}` | admin |
| 版本详情 | `openWorkflowVersionDetail(id)` [stainer-host.js:490](../backend/Stainer.Web/wwwroot/static/js/stainer-host.js#L490) | `GET /api/workflow-versions/{id}` | — | ⚠ 匿名 |
| 保存基本信息 | `updateWorkflowVersionMeta()` [stainer-host.js:1437](../backend/Stainer.Web/wwwroot/static/js/stainer-host.js#L1437) | `PUT /api/workflow-versions/{id}` | `{commandId, name?, description?, isEnabled?, versionLabel?, changeNote?}` | admin |
| 发布 Draft | `publishWorkflowVersion()` [stainer-host.js:1633](../backend/Stainer.Web/wwwroot/static/js/stainer-host.js#L1633) | `GET .../publish-validation` 后 `POST /api/workflow-versions/{id}/publish` | `{commandId}` | admin |
| 运行校验 | `validateWorkflowPublish()` | `GET /api/workflow-versions/{id}/publish-validation` | — | ⚠ 匿名 |
| 停用 | `retireWorkflowVersion(id)` | `POST /api/workflow-versions/{id}/retire` | `{commandId, reason}` | admin |
| 设为默认 | `setDefaultWorkflowVersion(id,type)` | `POST /api/workflow-versions/{id}/set-default` | `{commandId, experimentType}` | admin |
| 新增步骤 | `addWorkflowStep()` [stainer-host.js:1492](../backend/Stainer.Web/wwwroot/static/js/stainer-host.js#L1492) | `POST /api/workflow-versions/{id}/steps` | `{commandId, stepNo?, majorStepCode?, stepName, actionType, reagentCode?, volumeUl?, durationSeconds?, targetTemperatureDeciC?, mixParametersJson?, washParametersJson?, legacyParametersJson?, failureStrategy?}` | admin |
| 步骤编辑 | `editWorkflowStep(stepId)` | `PUT /api/workflow-versions/{id}/steps/{stepId}` | 同上 | admin |
| 步骤上移/下移 | `moveWorkflowStep(stepId,dir)` | `POST .../steps/{stepId}/move-up\|move-down` | `{commandId, preflightStateHash?}` | admin |
| 步骤删除 | `deleteWorkflowStep(stepId)` | `DELETE /api/workflow-versions/{id}/steps/{stepId}?commandId=` | —（query） | admin |
| 新增需求 | `addWorkflowRequirement()` | `POST /api/workflow-versions/{id}/reagent-requirements` | `{commandId, reagentCode, requiredVolumeUl?, isRequired}` | admin |
| 需求编辑/删除 | edit/delete | `PUT .../reagent-requirements/{id}` / `DELETE .../reagent-requirements/{id}?commandId=` | — | admin |
| 从步骤重算 | `recalculateWorkflowRequirements()` | `POST .../reagent-requirements/recalculate` | `{commandId}` | admin |
| 新增一抗映射 | `createPrimaryAntibodyMapping()` | `POST /api/primary-antibody-mappings` | `{commandId, primaryAntibodyCode, workflowVersionId}` | admin |
| 映射启用/停用 | enable/disable | `POST /api/primary-antibody-mappings/{id}/enable\|disable` | `{commandId, reason?}` | admin |
| 刷新 | `renderConfigure()` | `GET /api/workflows` + `/api/reagents/catalog` + `/api/primary-antibody-mappings` | — | ⚠ 匿名 |

### 1.10 `/engineer` 工程模式
| 控件 | 处理函数 | API | Body | 角色 |
|---|---|---|---|---|
| 二次认证 | `startEngineeringSession()` [engineer.js:19](../backend/Stainer.Web/wwwroot/static/js/engineer.js#L19) | `POST /api/engineering/session` | `{commandId, password, reason, target, durationMinutes}` | engineer/admin |
| 结束会话 | `revokeEngineeringSession()` [engineer.js:29](../backend/Stainer.Web/wwwroot/static/js/engineer.js#L29) | `POST /api/engineering/session/revoke` | `{commandId, reason, target, dangerousOperationConfirmed}` | engineer/admin |
| 刷新正式数据 | `loadEngineerPage()` [engineer.js:165](../backend/Stainer.Web/wwwroot/static/js/engineer.js#L165) | `GET /api/system/info` + `GET /api/runs/current` | — | 读 |
| 执行适配器测试 | `runEngineeringAdapterTest()` [engineer.js:35](../backend/Stainer.Web/wwwroot/static/js/engineer.js#L35) | `POST /api/device-initialization` | `{commandId}` | operator/engineer/admin |
| 刷新诊断 | `loadEngineeringDiagnostics()` [engineer.js:50](../backend/Stainer.Web/wwwroot/static/js/engineer.js#L50) | `GET /api/engineering/diagnostics/{device-state,command-log,errors,mock-communications}` | — | engineer/admin |
| 坐标版本 Diff | `showCoordinateDiff(id)` [engineer.js:116](../backend/Stainer.Web/wwwroot/static/js/engineer.js#L116) | `GET /api/engineering/coordinate-profile-versions/{id}/diff` | — | engineer/admin |
| 坐标复制 Draft | `createCoordinateVersion()` [engineer.js:137](../backend/Stainer.Web/wwwroot/static/js/engineer.js#L137) | `POST /api/engineering/coordinate-profile-versions` | `{commandId, profileCode, sourceVersionId?, versionLabel, reason, targetPoints[], validationResultJson?}` | engineer/admin + 工程会话 |
| 坐标 publish/activate/deactivate | `coordinateVersionAction(id,action)` [engineer.js:121](../backend/Stainer.Web/wwwroot/static/js/engineer.js#L121) | `POST /api/engineering/coordinate-profile-versions/{id}/{action}` | `{commandId, reason, target, dangerousOperationConfirmed}` | engineer/admin + 会话 |
| Liquid Class 复制/发布/启停 | `createLiquidClassVersion`/`liquidClassVersionAction` [engineer.js:147,125](../backend/Stainer.Web/wwwroot/static/js/engineer.js#L125) | `POST /api/engineering/liquid-classes` / `POST /api/engineering/liquid-class-versions/{id}/{action}` / `GET .../{id}/diff` | `{commandId, ...}` | engineer/admin + 会话 |
| 导出 JSON | `exportEngineeringConfig()` [engineer.js:152](../backend/Stainer.Web/wwwroot/static/js/engineer.js#L152) | `GET /api/engineering/config/export` | — | engineer/admin |
| 预览导入 | `previewEngineeringImport()` [engineer.js:158](../backend/Stainer.Web/wwwroot/static/js/engineer.js#L158) | `POST /api/engineering/config/import/preview` | `{configType, targetCode, payload}` | engineer/admin |
| 确认导入 | `applyEngineeringImport()` [engineer.js:161](../backend/Stainer.Web/wwwroot/static/js/engineer.js#L161) | `POST /api/engineering/config/import` | `{commandId, reason, target, configType, targetCode, payload, ...}` | engineer/admin + 会话 |
| 导出 CSV（命令/通讯） | `exportEngineeringCsv(type)` [engineer.js:151](../backend/Stainer.Web/wwwroot/static/js/engineer.js#L151) | `GET /api/engineering/diagnostics/command-log.csv` / `mock-communications.csv` | — | engineer/admin |
| 密码/原因/目标/时长/导入输入 | — | 喂给上述 | — | 客户端 |

### 1.11 `/admin` 系统管理
| 控件 | 处理函数 | API | Body | 角色 |
|---|---|---|---|---|
| 新增用户（工具栏） | `wireAdminToolbar`→`adminCreateUser()` [stainer-host.js:951](../backend/Stainer.Web/wwwroot/static/js/stainer-host.js#L951) | `POST /api/users` | `{commandId, username, displayName, password, roles[]}` | admin |
| 重置密码（工具栏） | `wireAdminToolbar`→`adminResetPassword(id)` [stainer-host.js:944](../backend/Stainer.Web/wwwroot/static/js/stainer-host.js#L944) | `PUT /api/users/{id}/password` | `{commandId, newPassword}` | admin |
| 改名 | `adminRenameUser(id)` | `PUT /api/users/{id}/display-name` | `{commandId, displayName}` | admin |
| 禁用/启用 | `adminToggleUser(id,enabled)` | `PUT /api/users/{id}/enabled` | `{commandId, enabled}` | admin |
| 重置（行内） | `adminResetPassword(id)` | `PUT /api/users/{id}/password` | `{commandId, newPassword}` | admin |
| 角色 | `adminSetRoles(id)` | `PUT /api/users/{id}/roles` | `{commandId, roles[]}` | admin |
| 删除 | `adminDeleteUser(id)` | `DELETE /api/users/{id}?commandId=` | —（query） | admin |
| 刷新 | `renderAdmin()` [stainer-host.js:335](../backend/Stainer.Web/wwwroot/static/js/stainer-host.js#L335) | 并行 `GET /api/users` `/api/roles` `/api/workflows` `/api/primary-antibody-mappings` `/api/reagents/catalog` `/api/alarms?status=Active` | — | admin（users/roles）；其余匿名 |
| 查询审计 | `loadTraceAudit()` | `GET /api/audit/logs?{filters}` | — | engineer/admin |
| 导出审计 CSV | `exportTraceCsv('audit')` | `GET /api/audit/export?{filters}` | — | engineer/admin |
| 9 个审计筛选 input | — | 作为 query 参数 | — | 客户端 |

### 1.12 `/mock-timeline`（仅 Dev/Testing）
开发用观察台，由 `mock-timeline.js` 驱动；生产隐藏。低优先级，此处略。

---

## 2. 缺口分析

### 2.1 线上 UI 中「无后端 API」的控件
线上 `LegacyUiPageRenderer` 渲染的控件**几乎全部已接线**。仅有的真缺口：
- **Dashboard 告警条「查看处理」按钮**无 onclick（装饰性）——补 `onclick="location.href='/alerts'"`。
- **`/control-console` 数字孪生页**（`twin/index.html`，747KB）的交互控件未被本次梳理覆盖；它仅调用 4 个 API，其余多为客户端 Mock。若需把孪生的控件（温控/液路/扫码/移液调试）接入真实 API，需单独梳理该打包页。

> 注：UI agent 初版报告里的 `engineerCommand`/`testPipette`/`testPump`/`safeMock`、configure「导入目录/导入配置包」、alerts「确认选中告警」、admin「导出审计 toast」等「死控件」**均来自旧 Python 模板**，线上 HTML 已不存在，不属于真缺口。

### 2.2 后端有 API、但**无任何 Web UI 消费**的端点（孤儿能力）
约 **50+ 个**端点已实现但 operator/twin 前端均不调用（对应近期「backend capabilities」先行提交）。分组：

| 域 | 孤儿端点 |
|---|---|
| 设备模式 | `GET /api/device-mode`、`POST /api/device-mode/change` |
| 设备/温控/液路 | `GET /api/device/state`；`/api/thermal/*`（6：state/telemetry/points/boards/cooling/faults[/clear]）；`/api/fluidics/*`（9：state/telemetry/pumps run+stop/wash/mixers start+complete+stop/liquid-levels/faults[/clear]） |
| Mock 故障 | `POST /api/device/mock-faults`[/clear] |
| 硬件扫码器 | `/api/device/reagent-scanner/qr/{reset,start,text,status,report,clear}`（6） |
| 启动/数据库 | `GET /api/executor/lease`、`POST /api/startup/recovery`、`GET /api/prehardware-readiness`、`GET /api/database/maintenance`、`POST /api/database/backup`、`POST /api/database/restore-request` |
| 身份 | `GET /api/current-user`（UI 用 snapshot 代替） |
| 工作流子列表 | `GET /api/workflows/{id}`、`GET /api/workflow-versions/{id}/steps`、`GET .../reagent-requirements`（UI 用内嵌详情） |
| DAB（大部分） | `GET /api/dab`、`GET /api/dab/positions`、`POST /api/dab/batches`、`GET /api/dab/batches/{id}`、`preparation/start`、`expire`、`fail`（仅 cleaning/start+confirm 被调用） |
| 工程（多） | `GET /api/engineering/layout`、`coordinate-profile-versions/{id}` 单查、`liquid-class-versions/{id}` 单查、`pipetting-tests/{types,liquid-detect,aspirate,dispense,wash,flush}`（6）、`coordinates/digital-twin/import{,/preview}`（2）、`coordinate-points/calibrate`、`device-profiles` |
| 扫码器配置 | `/api/scanners/*` + `/api/scanner-regions/*` + `/api/reagent-coordinate-anchors/*`（约 17） |
| 数字孪生 | `GET /api/twin/value/{id}`、`GET /api/twin/mapping`、`GET /api/twin/mapping.csv`（仅 snapshot 被调用） |
| Mock 演示 | `POST /api/mock-demo-data/{seed,reset}` |
| 遗留兼容 | `POST /api/system/initialize`、`POST /api/samples/scan`、`POST /api/reagents/scan`、`POST /api/run/{start,pause,resume,stop}`、`POST /api/slides/configure`、`POST /api/run/add-slide`、`POST /api/engineer/command`、`GET /api/state` |

**建议**：这些不是「补 API」而是「补 UI 或明确废弃」。优先级取决于产品规划——温控/液路/扫码器配置/pipetting 是数字孪生后续要接入的候选；遗留兼容端点（Python 时代）可逐步删除。

---

## 3. 统一方案

### 3.1 请求格式统一
| 问题 | 现状 | 建议 |
|---|---|---|
| `commandId` 位置 | 3 个 DELETE 走 query（`/api/users/{id}`、`.../steps/{stepId}`、`.../reagent-requirements/{id}`），其余在 body | 统一放 **body**。ASP.NET 支持 DELETE `[FromBody]`；改前端 3 处 + 后端 3 处签名 |
| 命名风格 | 遗留 dev-only DTO 用 snake_case（`slide_id`/`protocol_code`/`volume_ul`/`temperature_c`）；`/api/twin/value` 返回 `control_id` | 删除或改 camelCase。`/api/slides/configure` 的唯一消费者 `configure.js` 不被加载 → 连同端点一起删；`/api/engineer/command`、`/api/run/add-slide` 同属 dev-only 遗留，建议清理 |
| fetch helper | `run.js` 的 `fetchJsonOrNull` 与 `api()` 并存 | 合并到 `api.js`，加 `{tolerate404:true}` 选项 |
| 写返回体 | 各端点返回形状不一 | 统一为 `{ ok, commandId, ...payload }`（或 ProblemDetails），前端少写特判 |

### 3.2 权限统一
| 问题 | 现状 | 建议 |
|---|---|---|
| 读端点大批匿名 | `/api/system/info`、`/api/device-mode`、`/api/workflows`(GET)、`/api/workflow-versions/{id}`(GET) 及 steps/reagent-requirements/publish-validation、`/api/primary-antibody-mappings`(GET)、`/api/protocols`、`/api/reagents/{catalog,rack,scan-sessions/overview}`、`/api/health/database`、`/api/executor/lease` 均匿名 | 统一要求 `operator/engineer/admin`（页面本就假设已登录）；`/api/current-user` 保持 401 友好可例外 |
| 历史页审计块对 operator 403 | `/api/audit/{logs,export}` 要 engineer/admin，但 `/history` 导航对 operator 可见 | 二选一：operator 只读放行审计查询；或前端按角色隐藏审计区 |
| 同一「通道批次」三种角色口径 | `channel-batches/active`、`workflow-selection` = operator/admin；`experiment-type-selection` = operator/engineer/admin | 统一为 operator/admin（或都放开 engineer），避免工程师换不了流程却换得了实验类型 |
| 运行控制角色割裂 | pause/resume/stop = operator/admin（无 engineer）；fault/redo = engineer/admin（无 operator） | 单角色会话下，工程师现场**无法停止运行**、操作员**不能记故障**。确认是否预期；若需「现场工程师能急停」则把 stop 放宽给 engineer |
| 鉴权实现 | 命令式 `RequireAnyRoleAsync` 散落 215 个 handler | （较大重构）引入 `AddAuthorization` + 策略 `RequireAuthorization("policy")`，让门禁可审计、减样板 |

---

## 4. 建议的实施顺序

1. **低风险快赢**：dashboard 告警条按钮接线；删 `configure.js` + `/api/slides/configure`（死代码）。
2. **权限收紧**：把匿名读端点统一改为要求登录（一批小改，需回归 operator/engineer/admin 三角色页）。
3. **格式统一**：`commandId` 全部进 body（3 处 DELETE）。
4. **历史审计可见性**：决定 operator 是否可见审计，前端按角色渲染或后端放行。
5. **运行控制角色**：确认 engineer 是否需要 stop 权限。
6. **（可选大改）** 引入 ASP.NET 授权策略；梳理孪生页控件接入温控/液路/扫码器真实 API。
