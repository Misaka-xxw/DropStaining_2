using System.IO;

namespace Stainer.Tests;

/// <summary>
/// 测试专用临时根目录。所有测试的 SQLite 临时库与临时文件都应放在 <see cref="TempRoot"/> 之下，
/// 而不是直接调用 <c>Path.GetTempPath()</c>。
///
/// 原因：全量集成测试（200+ 项）每项都会创建若干 SQLite 库文件；若落在系统盘（C:）会把磁盘
/// 写满，导致测试宿主长时间挂起（团队记忆 MEMORY.md「Test temp disk gotcha」）。
///
/// <see cref="TempRoot"/> 取测试输出目录（<see cref="AppContext.BaseDirectory"/>）所在驱动器下的
/// <c>\tmp\stainer-tests</c>，与构建/仓库同盘（本仓库为 D:），驱动器无关。这是确定性方案——
/// 不依赖环境变量重定向，也不依赖运行时机较晚、在测试宿主中不可靠的模块初始化器。
/// </summary>
public static class TestPaths
{
    public static readonly string TempRoot = CreateRoot();

    private static string CreateRoot()
    {
        var baseDirectory = Path.GetFullPath(AppContext.BaseDirectory);
        var root = Path.GetPathRoot(baseDirectory)
            ?? throw new InvalidOperationException($"Cannot determine the volume root for test output directory '{baseDirectory}'.");

        var safeRoot = Path.Combine(root, "tmp", "stainer-tests");
        Directory.CreateDirectory(safeRoot);
        return safeRoot;
    }
}
