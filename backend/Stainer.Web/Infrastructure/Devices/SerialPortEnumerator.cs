using System.IO.Ports;

namespace Stainer.Web.Infrastructure.Devices;

// 本机串口名枚举：只读名字（SerialPort.GetPortNames），不开端口、不连接、不碰设备，安全。
// 遵循约定：System.IO.Ports 仅存在于 Infrastructure/Devices 层；Application/Web 层通过本类型间接使用。
public static class SerialPortEnumerator
{
    public static IReadOnlyList<string> ListAvailablePortNames()
    {
        try
        {
            return SerialPort.GetPortNames()
                .Where(p => !string.IsNullOrWhiteSpace(p))
                .Select(p => p.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(p => p, StringComparer.OrdinalIgnoreCase)
                .ToList();
        }
        catch
        {
            // 枚举失败（权限/平台差异等）返回空列表；前端会回退到 COM1-4。
            return Array.Empty<string>();
        }
    }
}
