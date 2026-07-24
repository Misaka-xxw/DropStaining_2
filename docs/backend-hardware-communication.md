# 后端与硬件通信总览

> 本文汇总 DropStaining_2 上位机后端到真实硬件的通信能力：已实现哪些、走哪条通道、受什么安全约束，以及哪些尚未实现/后续需要。进度对齐 `.claude/skills/fill_task_table/references/task_table.md`。
>
> 约束（`project_context/PROJECT_CONTEXT.md` + `.claude/context_brief.md`）：真实写命令必须 **fail-closed**，**禁止猜协议或伪造实现**，**禁止偷偷回退 Mock**；写命令受**协议白名单 + 参数量程 + 上层安全门禁**三重约束；设备适配器**不直接写业务库**（落库由 Application Service 负责）。

## 1. 三条硬件通道

| 通道 | 物理链路 | 协议 | 承载能力 |
|---|---|---|---|
| **主控串口** | RS232/串口 | `IceImmunoSerialProtocol`（帧头 `0xA5`/尾 `0x5A`、CRC16-Modbus，依据 `冰免通讯协议 ver1.0.6`） | 制冷/加热/光耦液位/清洗泵 PWM/试剂 QR/混匀/系统与节点状态 |
| **DCR55 扫码器** | 独立串口 | `Dcr55Protocol`（单次触发文本 `RDCMXEV1,P11,P20`，要求 `\r\n`） | 样本条码扫描（**仅**单次触发+读条码，显式拒绝命令下发） |
| **SOCON Bridge** | 本机命名管道 → 独立 x86 进程 → SOCON SDK | 长度前缀 UTF-8 JSON（`SoconBridgeProtocol.md`） | 机械臂 MoveX/Y/Z、吸液/排液/液面探测（仅审核过的 **Z-SOPA** 模式） |

主控真实传输 `MainControllerSerialTransport` 与 DCR55 `Dcr55SerialTransport` 由 `CompositeDeviceByteTransport` 按 endpoint 路由；`System.IO.Ports` 只出现在 `Infrastructure/Devices`。

## 2. 已实现的后端↔硬件通信能力

### 2.1 主控串口（读 + 写）

| 能力 | 协议路径 | 实现位置 | 状态 |
|---|---|---|---|
| 试剂条码扫描（启动/复位/读） | 主控 `0x08/0x04`、`0x08/0x05`、`0x08/0x06`、`0x08/0x01` | `UnavailableRealDeviceAdapter.ScanReagentAsync` + `ReagentHardwareSink` 四步范式 | 已完成 |
| 清洗泵 PWM 写（单/全通道） | 主控 `0x07/0x02`、`0x07/0x04`（ID 0..3，值 -100..100） | `UnavailableRealDeviceAdapter.RunPumpAsync` + Transport 白名单 | 已完成 |
| 制冷（目标温度/开关读写） | 主控 `0x03/0x01..0x06` | `SetCoolingAsync` 双写+回读 | 已完成 |
| 主控状态上传读取 | `0x01/0x05` 运行时间、`0x01/0x08` 工作、`0x01/0x09` 节点、`0x04/0x09..0x0B` 加热板、`0x0A/0x02..0x03` 混匀、`0x07/0x06` PWM 检测、`0x05/0x04` 光耦 PUT | `IRealDeviceReadAdapter` + `ChannelHardwareStatusService` | 已完成（通道硬件状态映射待现场节点标定） |

### 2.2 DCR55 样本扫码

| 能力 | 实现 | 状态 |
|---|---|---|
| 样本条码触发+读取 | `UnavailableRealDeviceAdapter.ScanSampleAsync` → `ReceiveDcr55ResultAsync` → `Dcr55SerialTransport.ReceiveAsync` | 已完成（真发触发；`SampleScanWriteService` 正式流程仍偏 Mock、前端未完全接线） |

### 2.3 SOCON Bridge——机械臂运动/移液（复用已审核 Z-SOPA 路径）

统一动作通道：`IReagentHardwareActionClient`（`SoconReagentHardwareActionClient`）→ 命名管道 → Bridge 反射调用 SDK。请求为纯数值坐标（`ReagentHardwareActionRequest: Operation/Axis/XUm/YUm/SafeZUm/ActionZUm/VolumeUl`），**模块无关、不暴露 COM/NodeID/SDK 类型**。

| 能力 | 入口 | 数据源 | 状态 |
|---|---|---|---|
| 试剂位 XY/Z 现场移动 | `/api/engineering/reagent-position-config/{rackCode}/move-test` | `ReagentPositionConfig`（mm，仅 R1-R40） | 已完成（待现场标定） |
| 试剂位移液 吸/排/液面探测 | `/api/engineering/pipetting-tests/*`（`EngineeringPipettingService` Real 模式） | `CoordinatePoints`（µm） | 已完成（洗针 fail-closed） |
| **通用坐标点 XY/Z 现场移动**（玻片/A-B液/配液/清洗位） | `/api/engineering/coordinate-points/{pointCode}/move-test` | `CoordinatePoints`（µm，pointType 无关） | 已完成（待现场标定） |
| **双针运动原语** MoveXY/MoveZ(Z1,Z2)/Aspirate/Dispense | `SoconRobotMotionPrimitives`（DI 三选二：Real+Enabled?Socon:Unavailable） | last-position 状态 | 已完成（WashOuter fail-closed；待 Bridge 扩宽标定） |
| **双针工程隔离测试**（XY 平移 + 5 个原子动作） | `/api/engineering/robot-arm/move-test`、`/atomic-action` | `MotionControlService` + `RobotArmAtomicActionService` | 已完成（端点暂未接前端） |

> 通用坐标点移动、双针 SOCON 原语、双针工程端点为最近新增，均**默认关闭**：需 `Device:Mode=Real` + `Device:SoconReagentHardware:Enabled=true` +（双针）`Device:SoconRobotMotion:Enabled=true` 三者全开才真发。

### 2.4 串口调试（独立通道）

| 能力 | 入口 | 状态 |
|---|---|---|
| 真实 COM 字节收发（绕开设备链路） | `POST /api/engineering/serial-debug/exchange`（admin-only，`SerialDebugService`） | 已完成（COM8+CH340 环回实测） |

## 3. 写动作的统一安全范式

试剂/坐标点/双针三类 SOCON 写动作共用同一套外壳（镜像自 `ReagentPositionHardwareService`）：

1. **端点三重门禁**：admin 角色 + `EngineeringSessionService.RequireWriteSessionAsync`（二次认证 + reason + target + 活动运行时的危险确认）。
2. **幂等**：`CommandIdempotencyService.RunAsync`（`CommandReceipts` 表 + SHA256 请求哈希，重放检测）。
3. **Real-only / 默认关闭**：Mock 模式抛 409；Real 未开开关 → Unavailable/fail-closed，**绝不回退 Mock**。
4. **审计**：`AuditLog` 记录 actor/action/target/完成步骤。
5. **Bridge 端门禁**：`--enable-real-actions` 命令行 + 本机 `realReadOnlyEnabled && realActionsEnabled` 双开关 + 轴白名单 + 标定 + 速度/位置/体积软限位；失败 `BestEffortStop` 并转 Blocked。

## 4. 尚未实现 / 后续需要

### 4.1 代码就绪、待现场门禁（非协议缺失）
- **通用坐标点移动、双针运动原语**：代码已完成并通过 442 项回归，但真机动臂仍需 Bridge 端把已审核动作范围/标定/量程从 R1-R40 扩到玻片位(A-01..D-04)/配液位(M1-M8)/A-B液源瓶(DabA/DabB)/清洗位 + 现场标定 + 安全评审。
- **前端接线**：`coordinate-points/move-test`、`robot-arm/move-test`、`robot-arm/atomic-action` 三个工程端点暂未接到数字孪生前端按钮（当前仅 HTTP 可调）。
- **通道硬件状态映射**：`ChannelHardwareStatusService` 代码已完整，待 appsettings 配置现场节点索引（`Device:ChannelHardwareStatus:Enabled` + A-D `Mappings`）。

### 4.2 协议缺失，必须保持 fail-closed（不得伪造）
- **外壁清洗 WashOuter**：SOCON Bridge 与主控协议均无对应命令；`SoconRobotMotionPrimitives.WashOuterAsync` 永远 409 `robot_wash_not_socon_action`。需向厂商索取正式清洗协议或抓帧形成经审核协议。
- **供水模块控制**：出水温度/水量/流速协议未定义；排水/排毒泵（IO 0x06）主控自控、白名单显式拒绝；`WaterSupplyControlService` Real 下 fail-closed。
- **洗针（NeedleWash）Real 化**：`EngineeringPipettingService` Real 下洗针 fail-closed（同 WashOuter，待清洗协议）。

### 4.3 按设计 DB-only，不应下发（现状正确）
- 扫码器 COM 口/初始化/校准光/ROI：DCR55 真实串口只做单次触发（显式拒绝 Exchange），参数走 appsettings 或纯 DB。
- 机械臂/主控 COM 口设置：`SerialConnectionConfigService` 纯 DB 持久化，真实 Transport 读 appsettings；COM 通路验证由 `serial-debug/exchange` 覆盖。

### 4.4 测试补强
- 建议补 `CoordinatePointHardwareServiceTests` / `SoconRobotMotionPrimitivesTests` / `RobotArmEngineeringServiceTests`，复用 `SoconReagentHardwareActionClientTests` 的 fake 命名管道 server，无实物锁定命令序列。

## 5. 相关代码索引

- **协议/边界**：`Application/Devices/`（`MainControllerProtocol`、`IceImmunoSerialProtocol`、`Dcr55Protocol`、`RealDeviceReadBoundary`、`ReagentHardwareActionBoundary`）。
- **真实 Transport / Adapter**：`Infrastructure/Devices/`（`MainControllerSerialTransport`、`Dcr55SerialTransport`、`CompositeDeviceByteTransport`、`UnavailableRealDeviceAdapter`、`SoconReagentHardwareActionClient`、`SoconRobotMotionPrimitives`、`SerialDebugService`、`ReagentHardwareSink`）。
- **写动作服务**：`Application/Services/`（`ReagentPositionHardwareService`、`CoordinatePointHardwareService`、`EngineeringPipettingService`、`RobotArmAtomicActionService`、`RobotArmProcessActionService`、`RobotMotionPrimitives`、`RobotArmEngineeringService`、`MotionControlService`）。
- **端点**：`Infrastructure/Web/WebHostEndpointExtensions.{ReagentPositionConfig,CoordinatePoint,RobotArm,Engineering,DeviceOperations,Serial}.cs`。
- **安全外壳**：`CommandIdempotencyService`、`EngineeringSessionService`、`AuditLog`。
- **Bridge 进程**：`bridges/Stainer.SoconBridge/`（协议见 `SoconBridgeProtocol.md`）。
