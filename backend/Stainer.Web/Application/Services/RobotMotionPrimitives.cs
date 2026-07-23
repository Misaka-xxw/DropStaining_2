namespace Stainer.Web.Application.Services;

// 机械臂底层动作原语（Ports）。原子动作服务（RobotArmAtomicActionService）只依赖这套接口，
// 不直接耦合 MotionControlService / EngineeringPipettingService 的重 DB 上下文，也不触碰 SOCON Bridge。
// 第一阶段只提供 Mock 实现；后续接真实硬件时新增 SoconRobotMotionPrimitives 并在 DI 中替换注册即可，
// 原子动作编排与单测无需改动。
public interface IRobotMotionPrimitives
{
    /// <summary>移动 Z 轴到绝对高度（单位：微米 µm）。</summary>
    Task MoveZAsync(long zUm, CancellationToken cancellationToken = default);

    /// <summary>在当前位置吸入指定体积液体（单位：微升 µL）。</summary>
    Task AspirateAsync(int volumeUl, CancellationToken cancellationToken = default);

    /// <summary>在当前位置排出指定体积液体（单位：微升 µL）。</summary>
    Task DispenseAsync(int volumeUl, CancellationToken cancellationToken = default);

    /// <summary>执行外壁清洗动作（外壁冲洗，不涉及吸 / 排具体体积）。</summary>
    Task WashOuterAsync(CancellationToken cancellationToken = default);
}

// 一条原语调用的结构化记录，供 Mock 实现记录调用顺序、供单元测试断言动作顺序。
public sealed record RobotPrimitiveCall(string Kind, long? ZUm, int? VolumeUl)
{
    public static RobotPrimitiveCall MoveZ(long zUm) => new("MoveZ", zUm, null);
    public static RobotPrimitiveCall Aspirate(int volumeUl) => new("Aspirate", null, volumeUl);
    public static RobotPrimitiveCall Dispense(int volumeUl) => new("Dispense", null, volumeUl);
    public static RobotPrimitiveCall WashOuter() => new("WashOuter", null, null);
}

// 第一阶段 Mock 实现：不调用任何真实硬件 / SOCON，仅记录调用顺序并模拟成功。
// 满足“不实现真实硬件调用 / 保持 Mock 模式正常”的约束；同时其 Calls 列表直接服务于单元测试。
public sealed class MockRobotMotionPrimitives : IRobotMotionPrimitives
{
    public List<RobotPrimitiveCall> Calls { get; } = new();

    public Task MoveZAsync(long zUm, CancellationToken cancellationToken = default)
    {
        Calls.Add(RobotPrimitiveCall.MoveZ(zUm));
        return Task.CompletedTask;
    }

    public Task AspirateAsync(int volumeUl, CancellationToken cancellationToken = default)
    {
        Calls.Add(RobotPrimitiveCall.Aspirate(volumeUl));
        return Task.CompletedTask;
    }

    public Task DispenseAsync(int volumeUl, CancellationToken cancellationToken = default)
    {
        Calls.Add(RobotPrimitiveCall.Dispense(volumeUl));
        return Task.CompletedTask;
    }

    public Task WashOuterAsync(CancellationToken cancellationToken = default)
    {
        Calls.Add(RobotPrimitiveCall.WashOuter());
        return Task.CompletedTask;
    }
}
