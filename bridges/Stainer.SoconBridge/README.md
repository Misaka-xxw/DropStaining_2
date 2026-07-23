# Stainer SOCON Bridge

本目录是 SOCON 独立 x86 Bridge。默认离线；显式启用后可提供受限只读会话，以及经过独立双开门禁、NodeID 白名单、轴标定和软限位保护的试剂运动/移液动作。

## 项目边界

- Console Application，目标框架为 .NET Framework 4.5.2。
- 项目文件固定 `PlatformTarget` 为 `x86`。
- 不加入主 `Stainer.sln`。
- 不使用 NuGet，不引入第三方 JSON 库。
- 不复制厂商 SDK、Demo、驱动或授权文件；仅从本机部署目录反射加载 SDK。
- 默认不连接设备；未通过本地配置和启动参数双重启用时，真实读写均失败闭合。

## 本机配置

示例配置文件是 `SoconBridge.config.example.json`，仅包含：

```json
{
  "sdkDirectory": "",
  "diagnosticsEnabled": true
}
```

本机配置文件名为 `SoconBridge.config.local.json`，该文件只用于本机部署并被根 `.gitignore` 精确忽略。

SDK 路径解析优先级：

1. `SoconBridge.config.local.json` 的 `sdkDirectory`
2. 环境变量 `STAINER_SOCON_SDK_DIR`
3. 均不存在或为空时视为未配置

Bridge 不会把实际 SDK 绝对路径写入 Git 文件、README、协议文档、标准输出、日志或 IPC `details`。

## IPC

Bridge 仅创建 Windows Named Pipe，名称固定为 `Stainer.SoconBridge`。不配置 TCP、HTTP、WebSocket 或其他网络监听器。

Pipe 使用 `PipeDirection.InOut`，每次连接只处理一个请求，返回一个响应后关闭连接。消息格式为 4 字节小端长度前缀加 UTF-8 JSON，请求体最大 64 KB。

PipeSecurity 使用受保护 DACL，仅授予当前 Windows 用户 SID 和 LocalSystem SID。该 ACL 只作为本机进程边界，不替代未来生产环境需要的 SMB、防火墙或服务账户安全配置。若未来 Bridge 改为 Windows 服务运行，必须通过部署配置显式加入服务账户 SID，不得默认放宽 ACL。

## 命令

当前仅支持：

- `Ping`
- `GetBridgeStatus`
- `ValidateSdkDeployment`
- `OpenConfiguredReadOnlySession` / `CloseConfiguredReadOnlySession`
- `GetConfiguredNodeBasicStatus` / `GetConfiguredAxisPositions`
- `MoveConfiguredAxis`
- `AspirateConfigured` / `DispenseConfigured` / `DetectLiquidConfigured`
- `StopConfiguredAxis`

动作命令只接受逻辑轴角色和工程量，不接受 COM、NodeID 或 SDK 路径。任何其他命令返回 `success=false`、`message=NotSupported`。

## 真实动作门禁

动作必须同时满足：

1. Bridge 用 `--enable-real-actions` 启动。
2. 本机配置同时设置 `realReadOnlyEnabled=true`、`realActionsEnabled=true`。
3. SDK、USB2CAN 参数、NodeID 白名单和轴映射通过校验。
4. 目标轴已标记完成厂商标定。
5. `pipetteApiMode` 为当前审核支持的 `Z-SOPA`。
6. 位置、速度、体积和动作超时均落在 `actionLimits` 内。

任一条件不满足时不调用 SDK 动作。动作异常或 SDK 返回失败会阻塞当前 session。

## 部署前检查

`ValidateSdkDeployment` 只执行文件系统检查和原始 PE 头检查：

- 当前 Bridge 进程是否为 x86。
- SDK 目录是否已配置且存在。
- `SOCON.API.dll` 是否存在。
- `SOCON.Utility.dll` 是否存在。
- `can_bootloader.dll` 是否存在。
- `can_bootloader.dll` 的 COFF Machine 是否为 `0x014C`。
- `SOCON.ScEventBus.dll` 是否存在。
- `C1.C1Zip.4.dll` 是否存在。

缺少旧包运行期依赖 `SOCON.ScEventBus.dll` 或 `C1.C1Zip.4.dll` 时保留诊断告警；2026-06-15 正式包未携带这两个文件，因此该告警不再单独阻止 session 打开，核心 DLL/PE/托管类型校验仍为硬门禁。

## 自检

构建后运行：

```powershell
Stainer.SoconBridge.exe --self-test
```

自检只使用临时目录、假文件、假配置和可注入的架构检测器。自检结束后会清理临时目录。
