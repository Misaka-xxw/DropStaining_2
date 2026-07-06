namespace Stainer.Web.Application.Devices;

public static class IceImmunoSerialProtocol
{
    public const byte FrameHeader = 0xA5;
    public const byte Version = 0x01;
    public const byte FrameTail = 0x5A;
    public const byte RequestType = 0x01;
    public const byte ResponseType = 0x02;
    public const ushort Crc16ModbusPolynomial = 0x8005;
    public const ushort Crc16ModbusInitialValue = 0xFFFF;
    public const ushort Crc16ModbusXorOut = 0x0000;

    private const ushort ReflectedPolynomial = 0xA001;

    public static byte[] BuildRequestFrame(byte parentClass, byte subClass, ReadOnlySpan<byte> payload = default)
    {
        var dataLength = checked(3 + payload.Length);
        var frame = new byte[1 + 1 + 2 + dataLength + 2 + 1];
        frame[0] = FrameHeader;
        frame[1] = Version;
        frame[2] = (byte)(dataLength & 0xFF);
        frame[3] = (byte)(dataLength >> 8);
        frame[4] = parentClass;
        frame[5] = subClass;
        frame[6] = RequestType;
        payload.CopyTo(frame.AsSpan(7, payload.Length));

        var crc = CalculateCrc16Modbus(frame.AsSpan(4, dataLength));
        var crcOffset = 4 + dataLength;
        frame[crcOffset] = (byte)(crc & 0xFF);
        frame[crcOffset + 1] = (byte)(crc >> 8);
        frame[^1] = FrameTail;
        return frame;
    }

    public static ushort CalculateCrc16Modbus(ReadOnlySpan<byte> data)
    {
        var crc = Crc16ModbusInitialValue;
        foreach (var value in data)
        {
            crc ^= value;
            for (var bit = 0; bit < 8; bit++)
            {
                crc = (crc & 0x0001) == 0x0001
                    ? (ushort)((crc >> 1) ^ ReflectedPolynomial)
                    : (ushort)(crc >> 1);
            }
        }

        return (ushort)(crc ^ Crc16ModbusXorOut);
    }
}
