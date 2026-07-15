# 项目交接摘要

> ⚠️ **历史文档（已被步骤 6 取代）**：本摘要描述的是步骤 6 之前的架构。其中关于 `LegacyUiPageRenderer.cs`、`wwwroot/static/js/{api,stainer-host,dashboard,run,engineer,configure}.js`、`wwwroot/static/css/app.css` 以及旧多页面路由（`/dashboard`、`/samples`、`/run`、`/configure`、`/engineer`、`/admin` 等）的描述均已在步骤 6 删除：正式 UI 收敛为唯一的 `/control-console`（`wwwroot/twin/index.html`），早期 `src/` FastAPI/Jinja 原型也已删除。本文保留作为历史交接参考，上述引用均已失效。

* 项目名称：全自动冰冻切片染色机上位机（Stainer）
* 当前分支 / 最新提交：`master` / `7617075ce2e90cc1e43bfc5b618896a255cf882e`（`删除临时文件`，2026-06-30 09:49:03 +08:00）；本地比 `origin/master` 领先 1 个提交
* 工作区是否有未提交修改：有；`data/logs/runtime-20260630.jsonl` 和 `data/machine-executor.lock` 两个运行时文件发生变化，另有本交接文档 `docs/PROJECT_HANDOFF.md` 未跟踪；未发现未提交的业务代码修改
* 更新时间：2026-06-30 10:05:28 +08:00

# 当前已完成

* ASP.NET Core 正式宿主和本地 HMI
  * `backend/Stainer.Web/Program.cs` 是当前正式入口，默认仅监听 `http://127.0.0.1:5205`。
  * `backend/Stainer.Web/Infrastructure/Web/LegacyUiPageRenderer.cs` 负责服务端页面骨架；`wwwroot/static/js/stainer-host.js`、`api.js` 和 `wwwroot/static/css/app.css` 负责无 React/Vue 的前端交互与样式。
  * 已注册主控、检查、样本、试剂、运行、告警、历史、配置、工程和管理等页面路由；SignalR Hub 为 `/hubs/machine`。
  * 当前 `api.js`、`stainer-host.js` 的 `node --check` 均通过。

* 用户、角色、认证和审计
  * `UserSessionService`、`UserManagementService`、`PasswordHashService` 已提供登录、用户创建、启停、改名、重置密码、角色维护和会话读取。
  * `ReferenceDataSeeder.cs` 幂等创建 `operator`、`engineer`、`admin` 角色和开发账号；密码以 PBKDF2-SHA256 保存。
  * 关键写操作使用 `CommandIdempotencyService` 和 `CommandReceipt` 保持 `commandId` 幂等，并写入 `AuditLog`。
  * 相关验证集中在 `BusinessWriteApiIntegrationTests.cs`、`ReferenceDataSeederTests.cs` 和 `WebHostIntegrationTests.cs`。

* 流程配置、发布、停用、默认流程和一抗映射
  * `WorkflowMaintenanceService.cs` 已支持流程、版本、步骤、试剂需求、发布、停用和一抗映射维护。
  * 最新提交新增每种实验类型唯一默认流程：`WorkflowVersion.DefaultExperimentType`、唯一过滤索引和 Published 状态检查约束。
  * `POST /api/workflow-versions/{workflowVersionId}/set-default` 仅允许管理员把已启用的 Published HE/IHC 版本设为对应默认流程；当前默认版本禁止直接停用或退休。
  * `DefaultWorkflowSelectionIntegrationTests.cs` 覆盖默认替换、权限、非法状态、既有批次快照不变、幂等和数据库唯一约束。
  * 当前开发数据库已应用 `20260629094312_DefaultWorkflowVersionPerExperimentType`。默认 HE 为“测试 HE 流程”v1（`e9f6b47c-ac4e-49be-aeaa-b41cda600cb8`），默认 IHC 为“测试 IHC 001-A”v1（`d38f9d30-da54-4dd9-bb4f-58caa1f66ca7`）。一抗代码 `001` 已启用并映射到默认 IHC；未发现重复默认项或重复映射。

* 通道级脚本和任务创建
  * `ChannelBatch` 是实验类型、流程版本和流程快照的权威来源；`SlideTask` 中的流程字段仅作为兼容副本和历史数据保留。
  * `POST /api/channel-batches/experiment-type-selection` 只接受 HE/IHC，由后端绑定当前默认 Published 流程并冻结快照；显式传入 `workflowVersionId` 会被拒绝。
  * 未启动批次允许填写原因后更换实验类型；已启动或锁定批次禁止更换和追加玻片。
  * 同一通道容量为 1–4 张玻片；Slot 活动占用唯一；任务创建、占位和通道绑定在事务内完成。
  * IHC 兼容性按“确认一抗代码 -> 已启用映射 -> 当前 ChannelBatch 流程版本”判断，允许多个一抗代码映射到同一流程，不再要求同通道一抗代码完全相同。
  * `WorkflowAssignmentHistory` 记录 InitialSelection、PreStartChange、Locked；旧批次回填冲突会进入 `NeedsManualResolution`。
  * 相关验证集中在 `DefaultWorkflowSelectionIntegrationTests.cs`、`BusinessWriteApiIntegrationTests.cs`、`ChannelBatchWorkflowBackfillServiceTests.cs` 和 `WorkflowAssignmentHistoryTests.cs`。

* 样本和试剂页面正式数据联动
  * `/samples` 使用正式通道批次、默认流程选择和 HE/IHC 任务创建 API；流程、任务和 Slot 状态不以 localStorage 作为权威来源。
  * `/reagents` 使用 SQLite 试剂架、扫码会话和扫码确认数据；支持开始/完成扫码会话、单 R 位、按列及全架扫码入口。
  * 页面首次加载读取完整快照，SignalR 事件和重连会触发刷新。
  * 样本、试剂相关后端服务为 `TaskCreationService.cs`、`ReagentQueryService.cs`、`ReagentScanWriteService.cs`。

* 启动前校验和 Mock 运行闭环
  * `PreflightValidationService.cs` 校验任务、批次脚本、1–4 张容量、IHC 映射、扫码会话、试剂有效期和余量。
  * `MachineRunService.cs`、`RunControlService.cs` 和 `MachineExecutor.cs` 支持创建、启动、暂停、恢复、整机停止、故障注入和当前大步骤重做。
  * `MachineExecutor` 记录 Planned、CommandSent、Acknowledged、Completed、Failed、Unknown 命令状态，并持久化步骤、试剂消耗、DAB 和告警。
  * `StartupRecoveryService.cs` 在启动时把无法确认的已发送命令标记为 Unknown，不自动当作完成或自动恢复真实动作。
  * 相关验证集中在 `RuntimeLedgerExecutorTests.cs`、`PreHardwareSafetyTests.cs` 和 `BusinessWriteApiIntegrationTests.cs`。

* 历史、告警、审计和 CSV 导出
  * `TraceabilityQueryService.cs` 与 `/api/history/*`、`/api/alarms*`、`/api/audit/*` 提供正式数据库查询、详情追溯、告警确认和 CSV 导出。
  * 告警确认会更新数据库并写 `AlarmAction`、`AuditLog`；导出遵循筛选条件并审计。
  * `TraceabilityApiIntegrationTests.cs` 覆盖筛选、权限、告警确认、关联追溯和导出。

* 联调前安全能力
  * 默认 `Device:Mode=Mock`；Real 模式请求要求 engineer/admin、原因和审计，并提示修改配置后重启生效。
  * `MachineExecutorLeaseService.cs` 使用独占文件租约保证单一执行器所有权；非所有者实例进入只读状态。
  * `SafetyLogWriter.cs` 输出结构化运行、设备和错误日志；`DatabaseHealthChecker.cs` 检查完整性、Migration、外键、WAL 和 busy timeout。
  * `DatabaseMaintenanceService.cs` 提供完整性检查、备份和离线恢复请求审计；`Stainer.Operations` 提供联调前检查命令。
  * 当前开发数据库 `PRAGMA integrity_check` 返回 `ok`。

# 正在进行

* 当前开发目标
  * 从最近提交内容判断，当前目标是完成“样本页只选择 HE/IHC、后端自动绑定默认 Published 流程”的 10.8.1 收口，并把开发数据库迁移和参考数据准备到可手工验收状态。

* 已做到哪里
  * 代码、Migration、API、管理 UI、样本 UI、种子和自动化测试已进入本地提交 `7617075`。
  * 开发数据库已经应用最新 Migration，默认 HE/IHC 和 `001` 映射均已落库。
  * 最新提交前同一份业务代码曾完成 `dotnet build Stainer.sln`（0 警告、0 错误）和 77/77 xUnit 测试；本轮 Node 语法检查仍通过。

* 还缺什么
  * 需要处理当前异常存活但不监听端口的 `Stainer.Web.exe`/`dotnet` 进程，然后在干净进程环境中重新执行标准 build/test。
  * 需要完成 `/samples`、`/reagents`、`/run` 的浏览器手工验收，包括两窗口 SignalR、暂停/恢复、故障、重做和重启恢复。
  * 当前数据库有 3 个 ChannelBatch、1 个 SlideTask、3 个 Alarm、46 条 AuditLog，但没有 ReagentBottle，也没有 MachineRun；尚未形成一轮完整手工 Mock 运行数据。
  * 本地提交尚未推送到 `origin/master`。
  * 当前 README 和 Mock 验收文档尚未同步到最新正式 API/UI 行为。

# 最近代码变更

* 新增文件
  * `backend/Stainer.Tests/DefaultWorkflowSelectionIntegrationTests.cs`：默认流程管理、通道实验类型选择、预启动更换和 IHC 多一抗兼容的集成测试。
  * `backend/Stainer.Web/Infrastructure/Data/Migrations/20260629094312_DefaultWorkflowVersionPerExperimentType.cs`：增加默认实验类型字段、唯一索引和 Published 检查约束。
  * `backend/Stainer.Web/Infrastructure/Data/Migrations/20260629094312_DefaultWorkflowVersionPerExperimentType.Designer.cs`：上述 Migration 的 EF Core 模型元数据。
  * `data/logs/device-20260630.jsonl`、`data/logs/runtime-20260630.jsonl`：运行时结构化日志；属于操作产物，不应继续作为功能代码提交。

* 修改文件
  * `backend/Stainer.Tests/ReferenceDataSeederTests.cs`：增加默认 HE/IHC、`001` 映射和幂等断言。
  * `backend/Stainer.Tests/WebHostIntegrationTests.cs`：更新样本弹窗、资源版本和通道变更入口断言。
  * `backend/Stainer.Web/Application/ReadModels/BusinessWriteReadModels.cs`：增加默认流程与通道流程响应字段。
  * `backend/Stainer.Web/Application/ReadModels/WorkflowReadModels.cs`：流程查询结果返回 `DefaultExperimentType`。
  * `backend/Stainer.Web/Application/Requests/BusinessWriteRequests.cs`：增加设置默认流程、按实验类型选择通道流程的 DTO，并检测多余 `workflowVersionId`。
  * `backend/Stainer.Web/Application/Services/ChannelBatchWorkflowService.cs`：按 HE/IHC 绑定默认流程，执行预启动兼容校验、历史、审计和事件发布。
  * `backend/Stainer.Web/Application/Services/CommandIdempotencyService.cs`：支持默认流程响应重放及唯一冲突业务化。
  * `backend/Stainer.Web/Application/Services/WorkflowMaintenanceService.cs`：设置/替换默认流程，并保护默认流程不被停用或退休。
  * `backend/Stainer.Web/Domain/Entities/WorkflowVersion.cs`：新增 `DefaultExperimentType`。
  * `backend/Stainer.Web/Infrastructure/Data/Migrations/StainerDbContextModelSnapshot.cs`：同步默认流程字段、索引和约束。
  * `backend/Stainer.Web/Infrastructure/Data/ReferenceDataSeeder.cs`：首次缺省时把测试 HE/IHC 设为默认，并保持管理员后续选择不被重启覆盖。
  * `backend/Stainer.Web/Infrastructure/Data/StainerDbContext.cs`：映射新字段、唯一约束，并允许 Published 版本只更新默认标记和更新时间。
  * `backend/Stainer.Web/Infrastructure/Repositories/EfWorkflowReadRepository.cs`：查询映射默认实验类型。
  * `backend/Stainer.Web/Infrastructure/Web/LegacyUiPageRenderer.cs`：样本弹窗改为只选择 HE/IHC。
  * `backend/Stainer.Web/Infrastructure/Web/RuntimePageBridgeService.cs`：未启动已选批次允许显示更换入口。
  * `backend/Stainer.Web/Infrastructure/Web/WebHostEndpointExtensions.cs`：注册设置默认流程和通道实验类型选择 API。
  * `backend/Stainer.Web/Program.cs`：增加仅执行 Migration、参考种子和验证输出的 `--seed-reference-data` 分支，不进入 Kestrel。
  * `backend/Stainer.Web/wwwroot/static/css/app.css`：实验类型双选按钮样式。
  * `backend/Stainer.Web/wwwroot/static/js/stainer-host.js`：移除样本页单独选择流程版本，增加默认流程管理和自动绑定调用。
  * `tools/Stainer.Operations/Program.cs`：让 `backup-database` 在 Migration/Seed 前直接执行，避免备份命令先改库。
  * `data/logs/device-20260629.jsonl`、`data/logs/runtime-20260629.jsonl`、`data/machine-executor.lock`：最新提交中混入的运行时变化。

* 删除文件
  * `uIgYYf1LOHG3FxFwvfKubRi4qgZSJsTvz5c9RE3Y.tmp`：删除随机临时文件。

* 当前未提交差异
  * `data/logs/runtime-20260630.jsonl`：新增 2 条 2026-06-30 01:49 UTC 的启动恢复/租约获取日志。
  * `data/machine-executor.lock`：被当前存活进程占用，Git 无法读取其完整 diff。

* 关键设计决定及原因
  * 默认流程存放在 `WorkflowVersion`，并以 `DefaultExperimentType` 唯一索引保证每种实验类型最多一个默认 Published 版本；原因是默认选择属于版本级发布策略，而不是单张样本状态。
  * 样本页只提交 HE/IHC，服务端解析当前默认版本；原因是避免浏览器持有过期版本选择并维持 `ChannelBatch` 权威性。
  * 更换默认流程不修改既有 ChannelBatch 快照；原因是保证已选通道和历史运行可追溯、不可被后台配置漂移影响。
  * `backup-database` 绕过启动 Migration/Seed；原因是备份必须先于任何可能写库的升级动作。

# 构建与测试

* 标准构建命令：`dotnet build Stainer.sln`
* 标准测试命令：`dotnet test backend\Stainer.Tests\Stainer.Tests.csproj --no-build`
* 前端语法命令：`node --check backend\Stainer.Web\wwwroot\static\js\api.js`、`node --check backend\Stainer.Web\wwwroot\static\js\stainer-host.js`
* 最近一次已确认结果：同一份业务代码在最新提交前构建成功，0 警告、0 错误；xUnit 通过 77/77，失败 0，跳过 0。
* 本轮结果：两个 Node 检查均通过。
* 本轮 build/test 未能完成：
  * 为避免覆盖当前存活 Web 进程的 `bin` 文件，先后尝试自定义 `ArtifactsPath` 和系统临时源码副本。
  * 两次 `dotnet build` 都在进入编译前返回退出码 1，输出为“0 个警告、0 个错误”；因此本轮没有启动测试。
  * 两次构建在 10:00:22–10:01:25 派生了 80 个 `dotnet` 子进程。本轮已按精确时间窗口全部终止并删除临时目录；9:49 已存在的进程未处理。
  * 这是当前环境/进程状态问题的事实；是否由异常存活的 Web 宿主、MSBuild 节点复用或其他外部状态导致，尚无证据确认。

* 其他可运行命令
  * 启动正式 Web：`dotnet run --project backend\Stainer.Web\Stainer.Web.csproj`，默认 `http://127.0.0.1:5205`。
  * 一次性 Migration + Seed + 验证：构建后运行 `dotnet backend\Stainer.Web\bin\Debug\net9.0\Stainer.Web.dll --seed-reference-data`；执行前必须停服务并备份数据库。
  * 联调前检查：`dotnet run --project tools\Stainer.Operations -- verify-prehardware-readiness`。
  * 数据库备份：`dotnet run --project tools\Stainer.Operations -- backup-database --output <目录>`。
  * 恢复请求：`dotnet run --project tools\Stainer.Operations -- request-restore --backup <文件> --reason <原因>`。
  * 旧 JSON 导入：`dotnet run --project tools\Stainer.LegacyImporter -- --source-dir <目录> (--dry-run|--apply) [--database-url <连接>] [--report-path <文件>]`。

# 架构与约束

* 当前模块依赖关系
  * `Stainer.Web`：唯一正式 ASP.NET Core 宿主，包含 Domain、Application、Infrastructure 和无框架前端资源。
  * `Domain/Entities`：用户、流程、试剂、通道批次、任务、运行台账、DAB、告警和审计实体。
  * `Application`：请求/响应模型、仓储接口和业务服务；业务规则集中在 Service，不应放回页面 JavaScript。
  * `Infrastructure/Data` 与 `Infrastructure/Repositories`：EF Core + SQLite 映射、Migration、种子和仓储实现。
  * `Infrastructure/Web`：Minimal API 路由、页面渲染、SignalR 和正式状态桥接。
  * `Stainer.Tests`：引用 `Stainer.Web` 和 `Stainer.LegacyImporter`，以 xUnit、WebApplicationFactory 和临时 SQLite 做集成验证。
  * `Stainer.Operations`、`Stainer.LegacyImporter`：引用 `Stainer.Web` 的模型和服务，分别承担运维检查/备份与旧 JSON 导入。
  * `src/app`：旧 FastAPI/Jinja 原型，仅作为参考；不是正式运行依赖。

* 不应破坏的边界或约定
  * `ChannelBatch.SelectedWorkflowVersionId` 和 `WorkflowSnapshotJson` 是通道脚本权威来源；不得恢复单张 SlideTask 独立选脚本。
  * 已选流程必须 Published；默认 HE/IHC 每类最多一个；更换默认流程不得回写既有批次快照。
  * 同一 Drawer 同时最多一个活动 ChannelBatch；同通道只允许 1–4 张玻片；同一 Slot 只能有一个活动任务。
  * 批次启动后流程永久锁定，禁止更换实验类型、流程、快照或追加玻片。
  * IHC 兼容性必须通过已启用的 `PrimaryAntibodyWorkflowMapping` 校验，不能简化成一抗代码全相同。
  * Published 流程内容不可直接编辑；应复制为 Draft 后修改、校验、发布。
  * 所有关键写操作应保持事务、`commandId` 幂等、AuditLog 和 SignalR 事件。
  * Unknown 设备命令不得自动视为 Completed；Real 模式不得自动恢复动作。
  * localStorage 只能保存纯 UI 偏好，不能成为流程、任务、Slot、试剂或运行状态权威来源。

* 配置、硬件、模拟器和数据库状态
  * 当前目标框架为 .NET 9；本机 `dotnet` 为 9.0.300，Node 为 v24.14.0。
  * SQLite 默认路径为 `data/stainer.db`，可由 `STAINER_DATABASE_URL` 或连接字符串覆盖；连接自动开启 foreign_keys、WAL 和 busy_timeout。
  * 当前数据库完整性为 `ok`，最新 Migration 已应用，默认流程数据已存在。
  * 默认 `Device:Mode=Mock`，`RealHealthCheckComplete=false`。未实现真实主控板、机械臂、扫码器或 LIS 协议命令。
  * SignalR 仅允许本机已登录用户，engineer/admin 可进入工程事件组。
  * 页面仍由 `LegacyUiPageRenderer.cs` 中的大段 HTML 字符串和 `stainer-host.js` 驱动，尚未拆成独立前端工程。

# 已知问题与风险

* P0 - 事实：存在异常存活进程
  * `dotnet` PID 28940 和 `Stainer.Web.exe` PID 39240 自 09:49 起存活；5205、5207、7000、8000 均无监听。
  * `Stainer.Web.exe` 持有 `data/machine-executor.lock` 并写入运行日志，说明它至少执行过恢复和租约获取，但没有形成可访问 Web 服务。
  * 在确认命令行和归属前不应启动第二个正式实例，也不应直接覆盖默认 `bin` 输出。

* P0 - 事实：本轮标准构建无法复验
  * 两种隔离构建方式均无编译诊断地返回退出码 1，并异常派生大量 MSBuild/`dotnet` 进程；本轮生成的 80 个进程已清理。
  * 推测：可能与现有宿主/构建节点状态相关；目前没有足够证据锁定根因。

* P1 - 事实：运行时文件被 Git 跟踪
  * `data/logs/*.jsonl` 和 `data/machine-executor.lock` 已进入最新提交，并持续制造脏工作区；`.gitignore` 不能自动取消已跟踪文件。
  * 锁文件被独占时会使 `git diff`/`git status --untracked-files=all` 出现权限错误。

* P1 - 事实：本地提交未推送
  * `master` 比 `origin/master` 领先 1 个提交。远端尚不包含默认流程、自动绑定、Migration 和相关测试。

* P1 - 事实：文档明显过期
  * 根目录没有当前 C# 系统 README；`src/README.md` 描述旧 FastAPI 原型且当前显示为乱码。
  * `docs/aspnet-core-web-host.md` 仍称页面大量依赖 Mock/静态占位。
  * `docs/mock-runtime-validation-guide.md` 仍称样本按钮只弹 Mock toast、全架扫描伪造状态，并包含旧单任务 `selectedWorkflowVersionId` 示例。

* P1 - 事实：仍有 Mock/兼容 API 残留
  * `WebHostEndpointExtensions.cs` 仍暴露 `MockRuntimeStore` 端点，包括 `/api/dab`、`/api/system/initialize`、`/api/samples/scan`、`/api/reagents/scan`、`/api/slides/configure`、`/api/run/add-slide` 和 `/api/engineer/command`。
  * 当前前端仍会调用 `/api/dab`；`dashboard.js`、`configure.js`、`engineer.js`、`run.js` 也保留兼容调用。需要逐页确认哪些入口仍可见和是否会误改 Mock 状态。

* P1 - 事实：尚未接真实硬件
  * Real 模式目前只有权限、审计、健康门槛和重启生效语义；主控板、机械臂、扫码器和 LIS 仍无真实协议实现。

* P1 - 事实：开发库尚不具备完整手工运行物料
  * 当前 `reagent_bottles=0`、`machine_runs=0`。虽然默认流程和映射已准备好，但仍需正式扫码准备试剂瓶后才能完成 Mock 全流程验收。

* P2 - 事实：开发默认账号密码固定
  * Seeder 会为 `operator`、`engineer`、`admin` 初始化密码 `123456`。该配置只能用于本地开发，进入部署或联调环境前必须增加首次改密/安全配置策略。

* P2 - 事实：前端维护成本较高
  * `LegacyUiPageRenderer.cs`、`stainer-host.js` 承担多页面 HTML 和交互，页面边界弱；替换 UI 时应保留正式 API、DTO、SignalR 和权限协议，并逐步拆分页面模块。

* P2 - 事实：运维 CLI 行为需进一步收口
  * `request-restore` 已实现但未写入 `PrintHelp()`。
  * 除 `backup-database` 外，其他命令会先自动执行 Migration、旧批次回填、Seed 和启动恢复；调用未知命令也会先产生这些副作用。

* P2 - 事实：缺少浏览器级自动化证据
  * 仓库未发现 Playwright/Cypress 配置；现有 UI 测试主要是 API 集成和 HTML/JavaScript 字符串断言，1920×1080 单屏、双窗口同步和交互布局仍依赖手工验收。

# 下一步建议

1. 清理异常宿主进程
   * 目标：确认 PID 28940/39240 的完整命令行和启动来源，只停止本项目异常实例，释放执行器锁。
   * 涉及模块：进程管理、`MachineExecutorLeaseService`、启动脚本。
   * 完成判定：相关进程退出，5205/5207 无监听，锁文件可读取；随后后台启动一次服务可正常监听并返回 `/health` 200。

2. 恢复可重复构建测试基线
   * 目标：查明无诊断退出码 1 和大量 `dotnet` 子进程的原因，在干净终端运行标准命令。
   * 涉及模块：.NET SDK/MSBuild 环境、解决方案构建、测试宿主。
   * 完成判定：`dotnet build Stainer.sln` 0 错误；`dotnet test backend\Stainer.Tests\Stainer.Tests.csproj --no-build` 77/77 或更多全部通过；执行后无残留测试进程。

3. 清理仓库运行时文件跟踪
   * 目标：保留本地运行日志和租约功能，但从 Git 索引中移除 `data/logs/*.jsonl`、`data/machine-executor.lock`，不删除用户数据库和备份。
   * 涉及模块：`.gitignore`、仓库索引、日志和租约路径配置。
   * 完成判定：启动/测试后 `git status` 不再因日志或锁文件变脏，也不会因锁文件权限导致 Git 命令失败。

4. 更新当前 C# 项目文档
   * 目标：增加根 README，修订 `aspnet-core-web-host.md` 和 `mock-runtime-validation-guide.md`，删除旧单任务流程与 Mock toast 描述。
   * 涉及模块：`README.md`、`docs/`、运维命令说明。
   * 完成判定：新开发者仅按文档即可完成启动、默认流程选择、试剂扫码、Mock 运行、备份和恢复请求。

5. 完成正式手工验收数据和端到端运行
   * 目标：扫码准备默认 HE/IHC 所需试剂，分别创建任务，执行预检、运行、暂停/恢复、故障、重做、告警和重启恢复。
   * 涉及模块：样本、试剂、预检、运行、SignalR、历史/告警/审计。
   * 完成判定：形成至少一条 HE 和一条 IHC 完整 MachineRun 追溯链；两浏览器同步一致；重启后状态正确恢复。

6. 逐项关闭前端 Mock 兼容入口
   * 目标：盘点当前可见按钮和页面脚本，优先把 `/api/dab`、初始化、配置和工程命令接入正式服务或明确禁用。
   * 涉及模块：`WebHostEndpointExtensions.cs`、`MockRuntimeStore.cs`、`dashboard.js`、`configure.js`、`engineer.js`、`run.js`、`stainer-host.js`。
   * 完成判定：正式页面不再通过 MockRuntimeStore 改写业务状态；保留的演示端点只在 Development 且有显著隔离。

7. 收口部署安全和运维 CLI
   * 目标：移除固定开发密码风险，补全 CLI 帮助，并让只读/未知命令不隐式执行 Migration、Seed 或恢复。
   * 涉及模块：`ReferenceDataSeeder.cs`、认证配置、`Stainer.Operations/Program.cs`。
   * 完成判定：部署环境要求显式凭据初始化；CLI 每个命令的写入副作用清晰、可审计、可测试。

8. 审查并同步本地提交
   * 目标：在上述构建测试和仓库清理完成后审查 `7617075`，避免把运行日志/锁文件继续推送。
   * 涉及模块：Git 提交、远端分支。
   * 完成判定：工作区干净，提交内容只包含预期源码/Migration/测试/文档，随后推送并确认远端 CI 或本地验证通过。
