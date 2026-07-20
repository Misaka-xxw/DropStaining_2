using System.Buffers.Binary;
using System.Linq;
using System.Text;

namespace Stainer.Web.Application.Devices;

public static class MainControllerProtocol
{
    public const byte SystemClass = 0x01;
    public const byte HeatingClass = 0x04;
    public const byte OptocouplerClass = 0x05;
    public const byte PwmClass = 0x07;
    public const byte QrClass = 0x08;
    public const byte MixerClass = 0x0A;

    public static byte[] BuildWorkStatusRequest() => Build(SystemClass, 0x08);
    public static byte[] BuildNodeStatusRequest() => Build(SystemClass, 0x09);
    public static byte[] BuildRunTimeRequest() => Build(SystemClass, 0x05);
    public static byte[] BuildBoardTemperaturesRequest(byte boardId) => BuildBoardRequest(HeatingClass, 0x09, boardId);
    public static byte[] BuildBoardTargetTemperaturesRequest(byte boardId) => BuildBoardRequest(HeatingClass, 0x0A, boardId);
    public static byte[] BuildBoardSwitchStatesRequest(byte boardId) => BuildBoardRequest(HeatingClass, 0x0B, boardId);
    public static byte[] BuildPwmSpeedsRequest() => Build(PwmClass, 0x06);
    public static byte[] BuildMixerOriginRequest(byte boardId) => BuildBoardRequest(MixerClass, 0x02, boardId);
    public static byte[] BuildMixerRemainingCountRequest(byte boardId) => BuildBoardRequest(MixerClass, 0x03, boardId);
    public static byte[] BuildQrScanStatusRequest() => Build(QrClass, 0x06);
    public static byte[] BuildQrTextRequest() => Build(QrClass, 0x01);

    public static MainControllerAck ParseAck(IceImmunoFrame frame)
    {
        EnsureFrame(frame, frame.ParentClass, frame.SubClass, IceImmunoSerialProtocol.ResponseType);
        EnsurePayloadAtLeast(frame, 1);
        return new MainControllerAck(frame.Payload[0] == 0x01, frame.Payload[0], frame.Payload[1..]);
    }

    public static MainControllerWorkStatus ParseWorkStatus(IceImmunoFrame frame)
    {
        var data = EnsureSuccessResponse(frame, SystemClass, 0x08, 1);
        return new MainControllerWorkStatus(data[0]);
    }

    public static MainControllerNodeStatuses ParseNodeStatuses(IceImmunoFrame frame)
    {
        var data = EnsureSuccessResponse(frame, SystemClass, 0x09, 64);
        return new MainControllerNodeStatuses(data);
    }

    public static MainControllerRunTime ParseRunTime(IceImmunoFrame frame)
    {
        EnsureFrame(frame, SystemClass, 0x05, IceImmunoSerialProtocol.ResponseType);
        EnsurePayloadAtLeast(frame, 1);
        return new MainControllerRunTime(frame.Payload.ToArray());
    }

    public static MainControllerTemperatureBoard ParseBoardTemperatures(IceImmunoFrame frame, bool target)
    {
        var data = EnsureSuccessResponse(frame, HeatingClass, target ? (byte)0x0A : (byte)0x09, 9);
        return new MainControllerTemperatureBoard(data[0], ReadInt16Values(data.AsSpan(1), 4), target);
    }

    public static MainControllerSwitchBoard ParseBoardSwitchStates(IceImmunoFrame frame)
    {
        var data = EnsureSuccessResponse(frame, HeatingClass, 0x0B, 9);
        var values = ReadUInt16Values(data.AsSpan(1), 4);
        if (values.Any(v => v != 0 && v != 1))
        {
            throw Error(IceImmunoProtocolError.InvalidPayload, "Temperature switch values must be 0 or 1.");
        }
        return new MainControllerSwitchBoard(data[0], values);
    }

    public static MainControllerOptocouplerStatus ParseOptocouplerPut(IceImmunoFrame frame)
    {
        EnsureFrame(frame, OptocouplerClass, 0x04, IceImmunoSerialProtocol.RequestType);
        EnsurePayload(frame, 3);
        var value = BinaryPrimitives.ReadUInt16LittleEndian(frame.Payload.AsSpan(1, 2));
        if (value != 0 && value != 1)
        {
            throw Error(IceImmunoProtocolError.InvalidPayload, "Optocoupler value must be 0 (not triggered) or 1 (triggered).");
        }
        return new MainControllerOptocouplerStatus(frame.Payload[0], value, true);
    }

    public static MainControllerPwmSpeeds ParsePwmSpeeds(IceImmunoFrame frame)
    {
        EnsureResponse(frame, PwmClass, 0x06, 8);
        return new MainControllerPwmSpeeds(ReadUInt16Values(frame.Payload, 4));
    }

    public static MainControllerMixerValue ParseMixerOrigin(IceImmunoFrame frame)
    {
        EnsureResponse(frame, MixerClass, 0x02, 3);
        return ParseMixerValue(frame, MainControllerMixerValueKind.Origin);
    }

    public static MainControllerMixerValue ParseMixerRemainingCount(IceImmunoFrame frame)
    {
        EnsureResponse(frame, MixerClass, 0x03, 3);
        return ParseMixerValue(frame, MainControllerMixerValueKind.RemainingCount);
    }

    public static MainControllerQrScanStatus ParseQrScanStatus(IceImmunoFrame frame)
    {
        EnsureResponse(frame, QrClass, 0x06, 2);
        return new MainControllerQrScanStatus(BinaryPrimitives.ReadUInt16LittleEndian(frame.Payload));
    }

    public static MainControllerQrText ParseQrText(IceImmunoFrame frame)
    {
        var isPullResponse = frame.ParentClass == QrClass
            && frame.SubClass == 0x01
            && frame.MessageType == IceImmunoSerialProtocol.ResponseType;
        var isPutReport = frame.ParentClass == QrClass
            && frame.SubClass == 0x03
            && frame.MessageType == IceImmunoSerialProtocol.RequestType;
        if (!isPullResponse && !isPutReport)
        {
            throw Error(IceImmunoProtocolError.UnexpectedCommand, "The frame is not a QR text response or PUT report.");
        }

        var maximumLength = isPullResponse ? 512 : 1024;
        if (frame.Payload.Length > maximumLength || frame.Payload.Any(value => value > 0x7F))
        {
            throw Error(
                IceImmunoProtocolError.InvalidPayload,
                $"QR text must be 0..{maximumLength} bytes of ASCII data.");
        }

        return new MainControllerQrText(
            Encoding.ASCII.GetString(frame.Payload),
            isPutReport ? MainControllerQrTextSource.PutReport : MainControllerQrTextSource.PullResponse);
    }

    private static byte[] Build(byte parentClass, byte subClass) =>
        IceImmunoSerialProtocol.BuildRequestFrame(parentClass, subClass);

    private static byte[] BuildBoardRequest(byte parentClass, byte subClass, byte boardId) =>
        IceImmunoSerialProtocol.BuildRequestFrame(parentClass, subClass, [boardId]);

    private static MainControllerMixerValue ParseMixerValue(
        IceImmunoFrame frame,
        MainControllerMixerValueKind kind) =>
        new(frame.Payload[0], BinaryPrimitives.ReadUInt16LittleEndian(frame.Payload.AsSpan(1, 2)), kind);

    private static short[] ReadInt16Values(ReadOnlySpan<byte> bytes, int count)
    {
        var values = new short[count];
        for (var index = 0; index < count; index++)
        {
            values[index] = BinaryPrimitives.ReadInt16LittleEndian(bytes.Slice(index * 2, 2));
        }

        return values;
    }

    private static ushort[] ReadUInt16Values(ReadOnlySpan<byte> bytes, int count)
    {
        var values = new ushort[count];
        for (var index = 0; index < count; index++)
        {
            values[index] = BinaryPrimitives.ReadUInt16LittleEndian(bytes.Slice(index * 2, 2));
        }

        return values;
    }

    // Validates a response frame (parentClass/subClass, MessageType == Response), validates that
    // the leading ack-type byte is 0x01 (success), and returns the business data that follows it.
    // expectedDataLength is the required length of the business data EXCLUDING the ack byte, so the
    // full payload must be exactly expectedDataLength + 1 bytes.
    private static byte[] EnsureSuccessResponse(IceImmunoFrame frame, byte parentClass, byte subClass, int expectedDataLength)
    {
        EnsureFrame(frame, parentClass, subClass, IceImmunoSerialProtocol.ResponseType);
        EnsurePayload(frame, expectedDataLength + 1);
        if (frame.Payload[0] != 0x01)
        {
            throw Error(
                IceImmunoProtocolError.InvalidPayload,
                $"Response ack type is 0x{frame.Payload[0]:X2}; expected 0x01 (success).");
        }

        return frame.Payload.AsSpan(1, expectedDataLength).ToArray();
    }

    private static void EnsureResponse(IceImmunoFrame frame, byte parentClass, byte subClass, int payloadLength)
    {
        EnsureFrame(frame, parentClass, subClass, IceImmunoSerialProtocol.ResponseType);
        EnsurePayload(frame, payloadLength);
    }

    private static void EnsureFrame(IceImmunoFrame frame, byte parentClass, byte subClass, byte messageType)
    {
        if (frame.ParentClass != parentClass || frame.SubClass != subClass || frame.MessageType != messageType)
        {
            throw Error(
                IceImmunoProtocolError.UnexpectedCommand,
                $"Unexpected frame 0x{frame.ParentClass:X2}/0x{frame.SubClass:X2}/0x{frame.MessageType:X2}.");
        }
    }

    private static void EnsurePayload(IceImmunoFrame frame, int payloadLength)
    {
        if (frame.Payload.Length != payloadLength)
        {
            throw Error(
                IceImmunoProtocolError.InvalidPayload,
                $"Payload length is {frame.Payload.Length}; expected {payloadLength}.");
        }
    }

    private static void EnsurePayloadAtLeast(IceImmunoFrame frame, int minimumLength)
    {
        if (frame.Payload.Length < minimumLength)
        {
            throw Error(
                IceImmunoProtocolError.InvalidPayload,
                $"Payload length is {frame.Payload.Length}; expected at least {minimumLength}.");
        }
    }

    private static IceImmunoProtocolException Error(IceImmunoProtocolError error, string message) => new(error, message);
}

public sealed record MainControllerAck(bool Succeeded, byte ResponseCode, byte[] AdditionalData);
public sealed record MainControllerWorkStatus(byte Value);
public sealed record MainControllerNodeStatuses(byte[] Values);

// V1.0.4 identifies this payload as run time but does not define its width or unit.
public sealed record MainControllerRunTime(byte[] RawValue);

public sealed record MainControllerTemperatureBoard(byte BoardId, short[] ValuesCelsius, bool IsTarget);
public sealed record MainControllerSwitchBoard(byte BoardId, ushort[] Values);
public sealed record MainControllerOptocouplerStatus(byte ChannelId, ushort Value, bool IsPutReport)
{
    public bool IsTriggered => Value == 1;
}
public sealed record MainControllerPwmSpeeds(ushort[] ValuesRpm);
public sealed record MainControllerMixerValue(byte BoardId, ushort Value, MainControllerMixerValueKind Kind);
public sealed record MainControllerQrScanStatus(ushort Value);
public sealed record MainControllerQrText(string Text, MainControllerQrTextSource Source);

public enum MainControllerMixerValueKind
{
    Origin,
    RemainingCount
}

public enum MainControllerQrTextSource
{
    PullResponse,
    PutReport
}
