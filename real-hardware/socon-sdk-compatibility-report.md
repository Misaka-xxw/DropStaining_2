# SOCON API SDK 兼容性取证报告

- 任务编号:23D-0
- 取证日期:2026-07-07
- 取证范围:**仅**静态/隔离元数据与程序集加载验证;**未连接任何真实设备、USB2CAN、机械臂或安全 IO**
- 报告依据:本机 SOCON.API.SDK-20260515 包目录(厂商原始包,未做任何改动)

> 本报告只回答一个问题:**SOCON API SDK 是否可在 `.NET Framework 4.5.2 x86` 与 `.NET 9 Windows x86` 的独立探针项目中编译，并通过 `Assembly.LoadFrom` 完成隔离程序集加载**,以指导后续“直接集成 vs 独立 Bridge”的架构决策。
> **本报告不构成对真实机械臂动作可行性的任何承诺。**

---

## 1. SDK 与 Demo 本地路径

| 项目 | 实际路径(相对于工作目录) | 状态 |
| --- | --- | --- |
| SDK 目录 | `SOCON.API.SDK/` | 存在 |
| Demo 目录 | `SOCON.API.DEMO/` | 存在 |
| 厂商上位机安装包 | `SoftControl/` | 存在(仅记录,未使用) |

> 依指令要求,本报告仅记录**文件名**与**相对路径**,不记录本机绝对路径。

### 1.1 SDK 文件清单(`SOCON.API.SDK/`)

| 文件名 | 类型 | 说明 |
| --- | --- | --- |
| `SOCON.API.dll` | 托管程序集(MSIL/AnyCPU) | 主 API 程序集,87 个导出类型 |
| `SOCON.Utility.dll` | 托管程序集(MSIL/AnyCPU) | 工具程序集,16 个导出类型 |
| `SOCON.API.xml` | XML 文档 | 仅注释,非可执行 |
| `can_bootloader.dll` | **原生 x86 二进制** | CAN 通讯底层驱动,非托管 |

### 1.2 关键 DLL 属性(PE / 托管)

| 程序集 | `ProcessorArchitecture` | `ImageRuntimeVersion` | 托管 | 备注 |
| --- | --- | --- | --- | --- |
| `SOCON.API.dll` | MSIL(AnyCPU) | v4.0.30319 | 是 | 可被 x86/x64 CLR 加载 |
| `SOCON.Utility.dll` | MSIL(AnyCPU) | v4.0.30319 | 是 | 可被 x86/x64 CLR 加载 |
| `can_bootloader.dll` | **x86(原生)** | — | 否 | 若执行 CAN 通讯路径，承载该路径的进程需为 x86 |

---

## 2. Demo 项目元数据

来源:`SOCON.API.DEMO/SOCON.API.DEMO.csproj`(只读检查)。

| 项 | 值 |
| --- | --- |
| Target Framework | `.NETFramework,Version=v4.5.2` |
| PlatformTarget | `AnyCPU`(未显式指定 `x86`) |
| OutputType | WinExE(Windows Forms) |
| 引用方式 | **HintPath** 直接指向 SDK 目录的同级 DLL |
| 引用程序集 | `SOCON.API.dll`、`SOCON.Utility.dll`(均 MSIL) |

Demo 用法要点(只读自源码):`Mainform`/`DevNormal`/`DevSCM4`/`DevSOCA` 通过 `SCDevice`、`SCDeviceMA`、`SocaXY`、`TPDevice` 暴露的实例方法操作设备,**未使用独立 `ErrorCode` 枚举**,错误以 `string`/`bool`/`int` 形式返回。

---

## 3. 程序集引用与原生依赖

### 3.1 `SOCON.API.dll` 引用程序集

- `mscorlib 4.0.0.0`
- `System 4.0.0.0`
- `System.Core 4.0.0.0`
- `SOCON.Utility 1.0.0.0`

### 3.2 `SOCON.Utility.dll` 引用程序集

- `mscorlib 4.0.0.0`
- `System 4.0.0.0`
- `System.Core 4.0.0.0`
- `System.Data 4.0.0.0`
- `System.Drawing 4.0.0.0`
- `SOCON.ScEventBus 1.0.0.0`(**SDK 包内未提供**)
- `C1.C1Zip.4 4.1.20131.101`(**SDK 包内未提供,ComponentOne 第三方库**)

> **缺失依赖影响范围**:`SOCON.ScEventBus` 与 `C1.C1Zip.4` 仅被 `SOCON.Utility.dll` 中**个别类型**引用。本次反射 `GetTypes()` **全部 16 个类型均成功枚举**,说明缺失依赖不是 **类型级强制依赖**(可能仅在某些方法体内被调用),因此**元数据加载与编译期引用不受影响**;但**运行期若触发相关代码路径**,仍可能抛出 `FileNotFoundException`。本取证未触发这些路径,无法证伪其运行期影响。

### 3.3 原生依赖(P/Invoke)

`SOCON.API.dll` 中的 `CAN_Bootloader` 类型通过 **17 个 P/Invoke** 调用 `can_bootloader.dll`:

```
CAN_BL_ScanDevice / CAN_BL_OpenDevice / CAN_BL_CloseDevice
SetConfigValue / GetConfigValue
SendCANMessage / RecvCANMessage / SetRxPackageCallBack
CAN_BL_Init / CAN_BL_NodeCheck / CAN_BL_Erase / CAN_BL_Write / CAN_BL_Excute
CAN_BL_SetNewBaudRate / CAN_BL_GetBaudRate
SendDataMessage / GetSendDataResult
```

由于 `can_bootloader.dll` 为 **x86 原生**,这些 P/Invoke **只能在 x86 进程内执行**;在 x64 进程中会得到 `BadImageFormatException`。

---

## 4. 关键类型与命名空间

### 4.1 命名空间

`SOCON.API.dll` 导出 87 个类型,**全部位于 `SOCON.API` 命名空间**(无二级命名空间)。

### 4.2 指令要求的关键类型核对

| 类型 | 是否存在 | 完整名 |
| --- | --- | --- |
| `SCDevice` | ✅ | `SOCON.API.SCDevice` |
| `SCDeviceMA` | ✅ | `SOCON.API.SCDeviceMA`(继承自 `SCDevice`) |
| `TPDevice` | ✅ | `SOCON.API.TPDevice`(继承自 `SCDevice`,温控模块) |
| `ErrorCode` | ❌ | **不存在**;SDK 以方法返回值表达错误(见 §5) |

> 指令列出的一些"等价类型名"(如 `SOCADevice`、`SCM4Device`、`NormalDevice`、`TPNode`、`TPCan`)在 `SOCON.API.dll` 中**未发现**。实际等价类型是 `SOCON.API.SocaXY`(SOCA 双臂 XY)与 `SOCON.API.CAN_Bootloader`(CAN 底层),命名与手册略有差异。

### 4.3 `SCDevice` 继承体系

```
System.Object
└── SOCON.API.SCDevice            (146 个公共方法)
    ├── SOCON.API.SCDeviceMA      (多臂统一控制,21 个新增方法,含 InitAllArm)
    ├── SOCON.API.TPDevice        (温控,7 个新增方法)
    └── SOCON.API.SocaXY          (SOCA XY 设备)
```

---

## 5. 关键方法签名与错误返回模式

### 5.1 指令要求的 25 个方法核对

**全部 25 个方法均存在**,分布如下(节选):

| 方法 | 所在类型 | 重载数 |
| --- | --- | --- |
| `Connect(string,int)` | `SCDevice` / `SocaXY` | 2 |
| `OpenPort(int,int)` / `ClosePort()` | `SCDevice` / `SocaXY` | 2 / 2 |
| `Disconnect()` | `SCDevice` | 1 |
| `RegisteNodeID(ProtocolTypeEnum, List<int>)` | `SCDevice` | 1 |
| `RegisteDeviceType(int, DeviceTypeEnum)` | `SCDevice` | 1 |
| `SetPerMM(AxisTypeEnum, float)` | `SCDevice` | 1 |
| `GetXPos(int)` / `GetYPos(int)` / `GetZ(int)` | `SCDevice` | 1 / 1 / 2 |
| `CheckIsInited(int)` / `CheckHome(int)` | `SCDevice` | 1 / 1 |
| `WaitActionDone(int/List<int>, int)` | `SCDevice` / `SocaXY` | 4 |
| `InitX` / `InitY` / `InitZ` | `SCDevice` 等 | 2~3 |
| `InitAllArm(...)` | `SCDeviceMA` | 1 |
| `Move` / `MoveStep` / `MoveX` / `MoveY` / `MoveZ` | `SCDevice` 等 | 1~3 |
| `LiqDet` / `Aspirate` / `Dispense` | `SCDevice` 等 | 2 |

### 5.2 错误返回模式(重要)

**SDK 不使用 `ErrorCode` 枚举**,而是采用**多模式返回值**:

| 方法类别 | 返回类型 | 含义 |
| --- | --- | --- |
| 动作类(`Move*`/`Init*`/`Aspirate`/`Dispense`/`LiqDet`/`WaitActionDone`) | `string` | 空字符串或 `null` 通常表示成功;非空字符串为错误描述/状态码 |
| 连接类(`Connect`/`OpenPort`/`ClosePort`) | `bool` | 成功与否 |
| 状态查询类(`CheckIsInited`/`CheckHome`) | `bool` | 是否已初始化/已回零 |
| 位置查询类(`GetXPos`/`GetYPos`/`GetZ`/`GetPos`) | `float` | 当前坐标(mm),**仅在该节点已注册并通讯成功时有意义** |
| 配置类(`SetCANBaudrate`/`WaitACK`) | `int` | 状态码/剩余 ACK 数 |

> 上层 Adapter 必须以"`string.IsNullOrEmpty(...)` 判定动作成功,`bool` 判定连接/状态,`int` 判定 ACK"的策略包装,而不是用一个统一枚举。

### 5.3 关键签名样例(节选,完整签名见 §4.2 反射结果)

```csharp
namespace SOCON.API
{
    public class SCDevice
    {
        public bool Connect(string ipAddress, int port);
        public bool OpenPort(int comPort, int baudrate);
        public bool ClosePort();
        public void Disconnect();

        public void RegisteNodeID(Utility.ProtocolTypeEnum protocolType, List<int> nodeIDs);
        public void RegisteDeviceType(int nodeID, Utility.DeviceTypeEnum deviceType);
        public void SetPerMM(Utility.AxisTypeEnum axisType, float value);

        public bool CheckIsInited(int nodeID);
        public bool CheckHome(int nodeID);
        public float GetXPos(int nodeID);
        public float GetYPos(int nodeID);
        public float GetZ(int nodeID);

        public string InitX(int nodeID, float speedBymm, float speedStartBymm, ...);
        public string Move(int nodeID, float distance, float speed, bool ifRelativeDistance);
        public string Aspirate(int nodeID, float aspirationVol, float speed, ...);
        public string Dispense(int nodeID, float dispenseVol, float speed, ...);
        public string LiqDet(int nodeID, float speedBymm, float liqSpeedBymm, ...);
        public string WaitActionDone(int nodeID, int timeout);
    }

    public class SCDeviceMA : SCDevice
    {
        public string InitAllArm(float speedY, ..., int timeout, ...);
        public string MoveX(int nodeID, float distance, float speed, Utility.AlarmSettings alarmSettings);
        // ...
    }
}
```

> 真实签名**参数量极大**(部分方法 15+ 参数),上层集成时务必引用厂商手册逐项核对 NodeID、泵映射、坐标系与安全 IO;本报告**不**构成动作可行性结论。

---

## 6. 初始化器与 P/Invoke 风险

### 6.1 模块初始化器

- **0 个**(`SOCON.API.dll` 与 `SOCON.Utility.dll` 均无模块级 `.cctor`)。

### 6.2 类型静态初始化器(`.cctor`)

共 **8 个**类型具有静态构造函数,**存在静态初始化风险**(触碰到任意一个该类型的静态成员即会运行):

```
SOCON.API.SCDevice            ← 主类,触及任何静态成员即触发
SOCON.API.SocaXY
SOCON.API.CONSTDEF
SOCON.API.CAN_Bootloader      ← 内含 17 个 P/Invoke 声明
SOCON.API.Utility
SOCON.Utility.EncryptString
SOCON.Utility.MessageService
SOCON.Utility.Common+<>c
```

> **隔离加载验证**已证实:`SCDevice`、`CAN_Bootloader` 等的 `.cctor` **在 `GetTypes()` 与 `Assembly.LoadFrom` 阶段并未被运行时强制执行**(.NET 仅在第一次访问该类型静态/实例成员时才触发)。因此本次隔离加载"通过",**不代表**实例化或访问这些类型的静态字段一定安全。集成时若在主进程内直接引用,需假设 `.cctor` 可能尝试扫描 CAN 设备或加载原生驱动——这会进一步**强化**使用独立 Bridge 的理由。

### 6.3 P/Invoke 风险

- **17 个 P/Invoke** 集中在 `SOCON.API.CAN_Bootloader`,目标库为 `can_bootloader.dll`(x86 原生)。
- **进程位数约束**:若执行依赖 `can_bootloader.dll` 的 USB2CAN / CAN 通讯路径，承载该路径的进程必须是 x86；x64/AnyCPU（64 位首选）进程在调用这些 P/Invoke 时会抛 `BadImageFormatException`。
- 本取证**未调用任何 P/Invoke 方法**,只枚举了声明。

---

## 7. 探针验证结果

> 取证在系统临时目录 `%TEMP%\socon-probes\` 下建立两个独立探针项目,**不复制厂商 DLL 进入源码树**,运行时通过命令行参数指向 SDK 目录进行 `Assembly.LoadFrom`。所有探针**仅做 `LoadFrom` / `GetTypes` / `GetReferencedAssemblies`,未实例化任何 SOCON 类型,未访问任何静态成员,未调用任何 SDK 业务方法**。

### 7.1 探针 A — `.NET Framework 4.5.2 x86`

- 项目:`ProbeNet452X86.csproj`,TargetFramework=`net452`,PlatformTarget=`x86`,OutputType=`Exe`
- 编译命令:`MSBuild /p:Configuration=Release`
- 编译结果:**成功**(0 错误 0 警告)
- 运行时进程架构:**x86**(`Is64BitProcess=False`)
- 隔离加载验证:

| 步骤 | 结果 |
| --- | --- |
| `Assembly.LoadFrom("SOCON.API.dll")` | ✅ `LOAD_OK` |
| `Assembly.LoadFrom("SOCON.Utility.dll")` | ✅ `LOAD_OK` |
| `apiAsm.GetTypes()` | ✅ `count=87`(全部成功) |
| `utilAsm.GetTypes()` | ✅ `count=16`(全部成功) |
| `can_bootloader.dll` 文件存在 | ✅ |
| 进程位数 | x86 |

**结论 A:`.NET Framework 4.5.2 x86` 可编译并加载 SOCON API SDK。✅**

### 7.2 探针 B — `.NET 9 Windows x86`

- 项目:`ProbeNet9X86.csproj`,TargetFramework=`net9.0`,RuntimeIdentifier=`win-x86`,OutputType=`Exe`
- 编译命令:`dotnet build -c Release`
- 编译结果:**成功**(0 错误,1 警告 `SYSLIB0037`——`AssemblyName.ProcessorArchitecture` 在 .NET 9 已过时,属探针代码自身小问题,不影响结论)
- 运行时:CLR `9.0.5`,`FrameworkDescription=.NET 9.0.5`,**`Is64BitProcess=False`,`ProcessArchitecture=X86`**
- 隔离加载验证:

| 步骤 | 结果 |
| --- | --- |
| `Assembly.LoadFrom("SOCON.API.dll")` | ✅ `LOAD_OK`,`ImageRuntimeVersion=v4.0.30319` |
| `Assembly.LoadFrom("SOCON.Utility.dll")` | ✅ `LOAD_OK` |
| `apiAsm.GetTypes()` | ✅ `count=87` |
| `utilAsm.GetTypes()` | ✅ `count=16` |
| `can_bootloader.dll` 文件存在 | ✅ |
| 进程位数 | x86 |

**结论 B:`.NET 9 Windows x86` 探针可编译，并可通过 `Assembly.LoadFrom` 隔离加载 SOCON API SDK 程序集。✅**

> 本次未验证 Stainer 主项目对 SOCON SDK 的直接编译期引用兼容性；本结论仅证明独立 `.NET 9 Windows x86` 探针可完成程序集元数据加载与类型枚举。

> 由于 SOCON 两个托管程序集均为 MSIL，它们在 .NET Framework 与 .NET 9 上的独立探针中均可加载；真正的**位数约束来自原生 `can_bootloader.dll`（仅 x86）**。若执行依赖该 DLL 的 CAN / P/Invoke 路径，承载该路径的进程必须是 x86。

### 7.3 两探针共同点

- **无 `BadImageFormatException`**(因探针进程均为 x86)
- **无 `FileNotFoundException`**(SDK 包内两个托管 DLL 足够支撑元数据加载)
- **无 `ReflectionTypeLoadException`**(`GetTypes` 全量返回)
- **未触发**任何方法调用,因此缺失的 `SOCON.ScEventBus` / `C1.C1Zip.4` 在本轮验证中**未造成失败**(运行期是否失败超出本取证范围)。

---

## 8. 缺失依赖与精确异常清单

| 项 | 状态 | 影响 |
| --- | --- | --- |
| `.NET Framework 4.5.2 Targeting Pack` | 已安装 | net452 探针编译成功 |
| `.NET 9 x86 Runtime` | 已安装 | net9 探针运行成功(`9.0.5`) |
| `MSBuild` / `csc` / `dotnet` SDK | 已安装 | 编译均成功 |
| `SOCON.ScEventBus.dll` | **SDK 包内未提供** | 元数据加载不受影响;运行期触发相关代码路径可能 `FileNotFoundException` |
| `C1.C1Zip.4.dll`(ComponentOne) | **SDK 包内未提供** | 同上 |
| `can_bootloader.dll` | 提供（原生 x86） | 若执行依赖它的 CAN 通讯路径，承载该路径的进程需为 x86 |
| USB2CAN 驱动 / 设备 | 未连接 | 不影响加载;运行期调用 `CAN_BL_OpenDevice` 等会失败(本取证未调用) |

> 依指令,**未自动安装、下载或替代任何缺失依赖**。

---

## 9. 兼容性结论

| 目标 | 探针可编译 | 可隔离加载 | 备注 |
| --- | --- | --- | --- |
| `.NET Framework 4.5.2 x86` | ✅ | ✅ | 与厂商 Demo 同框架,兼容性最确定 |
| `.NET 9 Windows x86` | ✅ | ✅ | 仅验证独立探针通过 `Assembly.LoadFrom` 加载并枚举类型；未验证 Stainer 主项目的直接编译期引用兼容性；`SYSLIB0037` 警告不影响本次加载结论 |

> 两个目标**在元数据/隔离加载层面均通过**。但:**加载成功 ≠ 真实机械臂动作可行**。真实动作还需要 NodeID 注册、泵映射、安全 IO、坐标/Z 参数与现场校准——这些均超出本取证范围。

---

## 10. 推荐的后续架构

> **唯一推荐结论:选项 B —— 使用独立 `.NET Framework x86` Bridge 进程承载 SOCON API SDK。**

### 10.1 推荐理由(综合证据)

1. **原生位数约束**:`can_bootloader.dll` 为 x86 原生。若 SDK 执行依赖该 DLL 的 USB2CAN / CAN 通讯路径，承载该路径的进程必须是 x86；若直接集成进当前非 x86 的 Stainer 主项目，调用这些 CAN/P/Invoke 路径会得到 `BadImageFormatException`。
2. **静态初始化器风险**:`SCDevice`、`CAN_Bootloader` 等 8 个类型含 `.cctor`。本次 `Assembly.LoadFrom` 与 `GetTypes()` 未触发这些 `.cctor`；但首次实例化相关类型，或访问其静态成员时，可能触发类型静态初始化。其实际副作用尚未验证，不能排除设备访问、原生依赖加载或其他运行期风险。独立 Bridge 进程可在物理上隔离这类副作用，避免污染 Stainer 主进程稳定性。
3. **缺失运行期依赖**:`SOCON.Utility` 引用的 `SOCON.ScEventBus`、`C1.C1Zip.4` 在 SDK 包内未提供。独立 Bridge 可以在不影响主项目依赖图的前提下,单独管控这些厂商/第三方 DLL 的部署。
4. **不修改 Stainer 架构**:指令禁止修改主项目平台目标、Mock/Real Adapter、数据库、Migration、页面、API、运行流程、坐标模型与 SOCON 动作门禁。独立 Bridge 通过进程间通讯(例如 stdin/stdout JSON、本地命名管道或 localhost TCP)对接,**对主项目零侵入**。
5. **选 .NET Framework 而非 .NET 9 作为 Bridge 框架**:厂商 Demo 本身就是 `net452`,且厂商上位机 `SoftControl` 也基于 .NET Framework。Bridge 选 .NET Framework 4.5.2/4.8 x86 可获得**与厂商同一运行时**的最高确定性,规避 .NET 9 下潜在的 Windows Forms / GDI / 第三方库兼容差异。

### 10.2 架构示意(仅建议,本任务不实施)

```
[Stainer 主项目 .NET 9, 平台目标不变]
        │  IPC(命名管道 / localhost TCP / stdin-stdout JSON)
        ▼
[SOCON Bridge 进程: .NET Framework 4.5.2 x86]
        │  P/Invoke(can_bootloader.dll, x86)
        ▼
[USB2CAN → 机械臂]
```

### 10.3 后续仍需补齐的工作(不在本任务范围)

- 真实 NodeID 表、泵型号映射、SOCA/SOPA 通道映射;
- 安全 IO 矩阵与急停门禁逻辑;
- 各节点坐标模型、Z 参数、回零方向与速度;
- 现场校准流程(本取证**不涉及**)。

---

## 11. 明确禁止的动作(本取证已严格遵守)

- ❌ 未修改 Stainer 主项目的平台目标;
- ❌ 未修改任何现有 Mock / Real Adapter;
- ❌ 未修改数据库结构、Migration、页面、API、运行流程、坐标模型、SOCON 动作门禁;
- ❌ 未创建 Bridge,未创建 SOCON Adapter,未连接设备;
- ❌ 未调用任何 SDK 的连接 / 初始化 / 回零 / 移动 / 探液 / 吸液 / 加液 / 等待完成方法(包括但不限于 `Connect`、`OpenPort`、`ClosePort`、`Disconnect`、`InitX/Y/Z/AllArm`、`Move*`、`LiqDet`、`Aspirate`、`Dispense`、`WaitActionDone`);
- ❌ 未复制或提交任何厂商 DLL、Demo、驱动、授权文件到仓库;
- ❌ 未自动下载或替代任何缺失依赖;
- ❌ 未提交 Git。

---

## 12. 临时探针目录清理结果

| 临时路径 | 用途 | 清理状态 |
| --- | --- | --- |
| `%TEMP%\socon-probes\net452-x86\` | .NET Framework 4.5.2 x86 探针源码 + bin/obj | 已清理 |
| `%TEMP%\socon-probes\net9-x86\` | .NET 9 win-x86 探针源码 + bin/obj | 已清理 |
| `%TEMP%\socon-probes\*.ps1` | 元数据枚举/探针构建脚本 | 已清理 |
| `%TEMP%\metaenum\` | 一次性反射枚举工具(MSIL exe) | 已清理 |
| `%TEMP%\socon-probe-metadata\` | 早期 PE/PE 架构预检脚本 | 已清理 |

> 仓库内**仅新增**一份报告:`real-hardware/socon-sdk-compatibility-report.md`。无任何厂商 DLL、Demo、驱动或临时构建产物进入仓库。

---

## 13. 附:复现指令清单(只读,供审计)

> 以下命令在临时目录执行,**不向仓库写入**;执行前用 UTF-16 命令行参数传入 SDK 路径,以规避 PowerShell 5.x 脚本文件 GBK 编码导致中文路径损坏的已知问题。

```powershell
# A. .NET Framework 4.5.2 x86 探针(编译+运行)
msbuild ProbeNet452X86.csproj /p:Configuration=Release
.\bin\Release\ProbeNet452X86.exe "<SDK 目录>"

# B. .NET 9 win-x86 探针(编译+运行)
dotnet build ProbeNet9X86.csproj -c Release
.\bin\Release\net9.0\win-x86\ProbeNet9X86.exe "<SDK 目录>"

# C. 元数据枚举(纯反射,不调用任何方法)
powershell -File run-reflect.ps1 -SdkDir "<SDK 目录>"
```

---

## 14. 一句话结论

> **SOCON API SDK 的两个托管程序集可在 `.NET Framework 4.5.2 x86` 与 `.NET 9 Windows x86` 的独立探针中编译，并通过隔离 `Assembly.LoadFrom` 完成加载与类型枚举；但本次未验证 Stainer 主项目对 SDK 的直接编译期引用兼容性。若执行依赖原生 x86 `can_bootloader.dll` 的 USB2CAN / CAN 通讯路径，承载该路径的进程必须是 x86；同时 SDK 存在 17 个 P/Invoke 与 8 个类型静态初始化器风险。在“不得修改 Stainer 平台目标”的约束下，推荐采用独立 `.NET Framework x86` Bridge 架构（结论 B）。**