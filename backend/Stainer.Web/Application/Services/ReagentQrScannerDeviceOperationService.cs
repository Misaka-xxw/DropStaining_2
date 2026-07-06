using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Stainer.Web.Application.Devices;
using Stainer.Web.Application.ReadModels;
using Stainer.Web.Application.Requests;
using Stainer.Web.Domain.Entities;
using Stainer.Web.Infrastructure.Data;

namespace Stainer.Web.Application.Services;

public sealed class ReagentQrScannerDeviceOperationService(
    IDeviceAdapter deviceAdapter,
    StainerDbContext dbContext,
    DeviceCommunicationPersistenceService communicationPersistence,
    ReagentScanWriteService scanWriteService,
    IReagentBarcodeParser barcodeParser)
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
    private static readonly char[] PrefixTrimChars = [' ', '\t', '\r', '\n', ':', '|', ',', ';', '#', '-', '_'];

    public Task<ReagentQrDeviceOperationResponse> ResetAsync(
        ReagentQrCommandRequest request,
        AuthenticatedUser actor,
        CancellationToken cancellationToken = default) =>
        RunDeviceCommandAsync(request, actor, ReagentQrCommands.ResetScan, BuildProtocolParameters(ReagentQrCommands.ResetScan), cancellationToken);

    public async Task<ReagentQrDeviceOperationResponse> StartAsync(
        ReagentQrCommandRequest request,
        AuthenticatedUser actor,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var text = string.IsNullOrWhiteSpace(request.Text)
                ? await BuildMockScanTextAsync(request, cancellationToken)
                : NormalizeScannerText(request.Text);
            var resolved = await ResolveQrTextAsync(text, request.Position, cancellationToken);
            var parameters = BuildProtocolParameters(ReagentQrCommands.StartScan);
            parameters["text"] = resolved.Text;
            parameters["rawBarcode"] = resolved.RawBarcode;
            parameters["position"] = resolved.Position.Code;
            parameters["channelCode"] = resolved.Position.ScannerChannelCode;
            parameters["channelResolvedByMock"] = resolved.ChannelResolvedByMock;

            var result = await ExecuteDeviceCommandAsync(
                request.CommandId,
                request.ScanSessionId,
                actor,
                ReagentQrCommands.StartScan,
                parameters,
                cancellationToken);
            return ToOperationResponse(request.CommandId, ReagentQrCommands.StartScan, result, false);
        }
        catch (Exception ex)
        {
            return ToOperationFailure(request.CommandId, ReagentQrCommands.StartScan, ex);
        }
    }

    public Task<ReagentQrDeviceOperationResponse> ReadTextAsync(
        ReagentQrCommandRequest request,
        AuthenticatedUser actor,
        CancellationToken cancellationToken = default) =>
        RunDeviceCommandAsync(request, actor, ReagentQrCommands.GetText, BuildProtocolParameters(ReagentQrCommands.GetText), cancellationToken);

    public Task<ReagentQrDeviceOperationResponse> GetStatusAsync(
        ReagentQrCommandRequest request,
        AuthenticatedUser actor,
        CancellationToken cancellationToken = default) =>
        RunDeviceCommandAsync(request, actor, ReagentQrCommands.GetScanStatus, BuildProtocolParameters(ReagentQrCommands.GetScanStatus), cancellationToken);

    public async Task<ReagentQrReportResponse> ReportTextAsync(
        ReportReagentQrTextRequest request,
        AuthenticatedUser actor,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var text = NormalizeScannerText(request.Text);
            if (string.IsNullOrWhiteSpace(text))
            {
                var readResult = await ExecuteDeviceCommandAsync(
                    $"{RequireCommandId(request.CommandId)}-read",
                    request.ScanSessionId,
                    actor,
                    ReagentQrCommands.GetText,
                    BuildProtocolParameters(ReagentQrCommands.GetText),
                    cancellationToken);
                if (!readResult.Ok)
                {
                    return ToReportResponse(request.CommandId, ReagentQrCommands.PutText, readResult, null, null, false, false, readResult.Message);
                }

                text = NormalizeScannerText(Convert.ToString(readResult.Data.GetValueOrDefault("text")));
            }

            var resolved = await ResolveQrTextAsync(text, request.Position, cancellationToken);
            var parameters = BuildProtocolParameters(ReagentQrCommands.PutText);
            parameters["text"] = resolved.Text;
            parameters["rawBarcode"] = resolved.RawBarcode;
            parameters["position"] = resolved.Position.Code;
            parameters["channelCode"] = resolved.Position.ScannerChannelCode;
            parameters["channelResolvedByMock"] = resolved.ChannelResolvedByMock;

            var deviceResult = await ExecuteDeviceCommandAsync(
                request.CommandId,
                request.ScanSessionId,
                actor,
                ReagentQrCommands.PutText,
                parameters,
                cancellationToken);
            if (!deviceResult.Ok)
            {
                return ToReportResponse(request.CommandId, ReagentQrCommands.PutText, deviceResult, resolved, null, false, false, deviceResult.Message);
            }

            await EnsureBarcodeDefinitionAsync(resolved.RawBarcode, cancellationToken);
            var scanSessionId = request.ScanSessionId;
            if (string.IsNullOrWhiteSpace(scanSessionId))
            {
                var session = await scanWriteService.StartSessionAsync(
                    new StartReagentScanSessionRequest($"{RequireCommandId(request.CommandId)}-scan-session"),
                    actor,
                    cancellationToken);
                scanSessionId = session.Session.ScanSessionId;
            }

            var scanResult = string.IsNullOrWhiteSpace(resolved.RawBarcode)
                ? ReagentScanResult.Empty
                : ReagentScanResult.Valid;
            var confirmation = await scanWriteService.ConfirmScanAsync(
                new ConfirmReagentScanRequest(
                    $"{RequireCommandId(request.CommandId)}-scan-confirm",
                    [
                        new ReagentScanInputItem(
                            resolved.Position.Code,
                            scanResult,
                            resolved.RawBarcode,
                            resolved.Position.Code,
                            request.ExpirationDate ?? DateOnly.FromDateTime(DateTime.UtcNow.AddYears(1)),
                            null)
                    ],
                    scanSessionId),
                actor,
                cancellationToken);

            return ToReportResponse(
                request.CommandId,
                ReagentQrCommands.PutText,
                deviceResult,
                resolved,
                confirmation,
                true,
                confirmation.Replayed,
                confirmation.Message);
        }
        catch (Exception ex)
        {
            return ToReportFailure(request.CommandId, ReagentQrCommands.PutText, ex);
        }
    }

    public Task<ReagentQrDeviceOperationResponse> ClearTextAsync(
        ReagentQrCommandRequest request,
        AuthenticatedUser actor,
        CancellationToken cancellationToken = default) =>
        RunDeviceCommandAsync(request, actor, ReagentQrCommands.ClearText, BuildProtocolParameters(ReagentQrCommands.ClearText), cancellationToken);

    private async Task<ReagentQrDeviceOperationResponse> RunDeviceCommandAsync(
        ReagentQrCommandRequest request,
        AuthenticatedUser actor,
        string command,
        IReadOnlyDictionary<string, object?> parameters,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await ExecuteDeviceCommandAsync(
                request.CommandId,
                request.ScanSessionId,
                actor,
                command,
                parameters,
                cancellationToken);
            return ToOperationResponse(request.CommandId, command, result, false);
        }
        catch (Exception ex)
        {
            return ToOperationFailure(request.CommandId, command, ex);
        }
    }

    private async Task<DeviceCommandResult> ExecuteDeviceCommandAsync(
        string commandId,
        string? correlationId,
        AuthenticatedUser actor,
        string command,
        IReadOnlyDictionary<string, object?> parameters,
        CancellationToken cancellationToken)
    {
        var operationRequest = new DeviceOperationRequest(
            new DeviceCommandContext(RequireCommandId(commandId), correlationId, actor.Username, nameof(ReagentQrScannerDeviceOperationService)),
            DeviceModules.ReagentScanner,
            command,
            parameters);
        var communicationRecord = communicationPersistence.Begin(operationRequest);
        await dbContext.SaveChangesAsync(cancellationToken);
        var result = await deviceAdapter.ScanReagentAsync(operationRequest, cancellationToken);
        await communicationPersistence.TryPersistCompletionAsync(communicationRecord, result, cancellationToken);
        return result;
    }

    private async Task<string> BuildMockScanTextAsync(ReagentQrCommandRequest request, CancellationToken cancellationToken)
    {
        var position = await ResolveRequestedPositionAsync(request.Position, cancellationToken)
            ?? await FirstEnabledPositionAsync(cancellationToken);
        var rawBarcode = string.IsNullOrWhiteSpace(request.RawBarcode)
            ? $"HEM050{DateTime.UtcNow:yyyyMMdd}{position.PositionNo % 1000:000}"
            : request.RawBarcode.Trim();
        return $"{position.ScannerChannelCode}:{rawBarcode}";
    }

    private async Task<ResolvedReagentQrText> ResolveQrTextAsync(
        string? text,
        string? requestedPosition,
        CancellationToken cancellationToken)
    {
        var normalizedText = NormalizeScannerText(text);
        var rawBarcode = ExtractRawBarcode(normalizedText);
        var prefix = ExtractPrefix(normalizedText, rawBarcode);
        var position = await ResolvePositionAsync(requestedPosition, prefix, cancellationToken);
        return new ResolvedReagentQrText(
            normalizedText,
            rawBarcode,
            prefix,
            position.Position,
            position.ChannelResolvedByMock);
    }

    private async Task<ResolvedPosition> ResolvePositionAsync(
        string? requestedPosition,
        string? prefix,
        CancellationToken cancellationToken)
    {
        var byRequest = await ResolveRequestedPositionAsync(requestedPosition, cancellationToken);
        if (byRequest is not null)
        {
            return new ResolvedPosition(byRequest, false);
        }

        var normalizedPrefix = prefix?.Trim(PrefixTrimChars);
        if (!string.IsNullOrWhiteSpace(normalizedPrefix))
        {
            var positionPrefix = normalizedPrefix.ToUpperInvariant();
            var channelPrefix = normalizedPrefix.ToLowerInvariant();
            var byCode = await dbContext.ReagentRackPositions
                .SingleOrDefaultAsync(x => x.Code == positionPrefix, cancellationToken);
            if (byCode is not null)
            {
                return new ResolvedPosition(byCode, false);
            }

            var channelQuery = dbContext.ReagentRackPositions.Where(x => x.ScannerChannelCode == channelPrefix);
            if (int.TryParse(normalizedPrefix.TrimStart('0'), out var channelNo))
            {
                channelQuery = dbContext.ReagentRackPositions.Where(x => x.ScannerChannelNo == channelNo);
            }

            var byChannel = await channelQuery
                .Where(x => x.IsEnabled)
                .OrderBy(x => x.PositionNo)
                .FirstOrDefaultAsync(cancellationToken);
            if (byChannel is not null)
            {
                return new ResolvedPosition(byChannel, true);
            }

            throw new BusinessRuleException(
                "reagent_qr_channel_not_found",
                "Reagent QR channel prefix was not found in reagent rack positions.",
                StatusCodes.Status400BadRequest);
        }

        return new ResolvedPosition(await FirstEnabledPositionAsync(cancellationToken), true);
    }

    private async Task<ReagentRackPosition?> ResolveRequestedPositionAsync(string? requestedPosition, CancellationToken cancellationToken)
    {
        var positionCode = requestedPosition?.Trim().ToUpperInvariant();
        if (string.IsNullOrWhiteSpace(positionCode))
        {
            return null;
        }

        var position = await dbContext.ReagentRackPositions
            .SingleOrDefaultAsync(x => x.Code == positionCode, cancellationToken);
        if (position is null)
        {
            throw new BusinessRuleException("reagent_position_not_found", "Reagent rack position was not found.", StatusCodes.Status404NotFound);
        }

        return position;
    }

    private async Task<ReagentRackPosition> FirstEnabledPositionAsync(CancellationToken cancellationToken)
    {
        var position = await dbContext.ReagentRackPositions
            .Where(x => x.IsEnabled)
            .OrderBy(x => x.PositionNo)
            .FirstOrDefaultAsync(cancellationToken);
        if (position is null)
        {
            throw new BusinessRuleException("reagent_position_not_found", "No enabled reagent rack position was found.", StatusCodes.Status404NotFound);
        }

        return position;
    }

    private async Task EnsureBarcodeDefinitionAsync(string? rawBarcode, CancellationToken cancellationToken)
    {
        var parsed = barcodeParser.Parse(rawBarcode);
        if (!parsed.IsValid || string.IsNullOrWhiteSpace(parsed.ReagentCode))
        {
            return;
        }

        if (await dbContext.ReagentDefinitions.AnyAsync(x => x.ReagentCode == parsed.ReagentCode, cancellationToken))
        {
            return;
        }

        var first = parsed.ReagentCode[0];
        if (first != 'F' && first != 'S' && !char.IsDigit(first))
        {
            return;
        }

        var now = DateTimeOffset.UtcNow;
        dbContext.ReagentDefinitions.Add(new ReagentDefinition
        {
            ReagentCode = parsed.ReagentCode,
            Name = KnownReagentName(parsed.ReagentCode),
            ReagentType = ReagentType(parsed.ReagentCode),
            LegacyMetadataJson = JsonSerializer.Serialize(new
            {
                source = "reagent_qr_device_operation",
                barcodeRule = "IceImmunoReagentBarcodeVer1_0_00",
                createdFromScan = true
            }, JsonOptions),
            IsEnabled = true,
            CreatedAtUtc = now
        });
    }

    private static string NormalizeScannerText(string? text)
    {
        return (text ?? string.Empty).Trim('\0', ' ', '\t', '\r', '\n');
    }

    private static string? ExtractRawBarcode(string text)
    {
        if (!string.IsNullOrWhiteSpace(text) && text.Length < ReagentBarcodeParser.BarcodeLength)
        {
            return text.Trim();
        }

        if (text.Length < ReagentBarcodeParser.BarcodeLength)
        {
            return null;
        }

        return text[^ReagentBarcodeParser.BarcodeLength..].Trim();
    }

    private static string? ExtractPrefix(string text, string? rawBarcode)
    {
        if (string.IsNullOrWhiteSpace(rawBarcode) || text.Length == rawBarcode.Length)
        {
            return null;
        }

        var prefix = text[..^rawBarcode.Length].Trim(PrefixTrimChars);
        return string.IsNullOrWhiteSpace(prefix) ? null : prefix;
    }

    private static Dictionary<string, object?> BuildProtocolParameters(string command)
    {
        var parameters = new Dictionary<string, object?>
        {
            ["protocol"] = "IceImmunoSerialFrame",
            ["frameHeader"] = "0xA5",
            ["version"] = "0x01",
            ["requestType"] = "0x01",
            ["responseType"] = "0x02",
            ["crcAlgorithm"] = "CRC16-MODBUS",
            ["crcPolynomial"] = "0x8005",
            ["crcInitialValue"] = "0xFFFF",
            ["crcXorOut"] = "0x0000",
            ["crcInputReflected"] = true,
            ["crcOutputReflected"] = true,
            ["crcRange"] = "data-area",
            ["endianness"] = "little"
        };

        if (command == ReagentQrCommands.StartScan)
        {
            parameters["parentClass"] = "0x08";
            parameters["subClass"] = "0x04";
            parameters["requestFrameHex"] = Convert.ToHexString(IceImmunoSerialProtocol.BuildRequestFrame(0x08, 0x04));
            parameters["mappingSource"] = "confirmed";
        }
        else if (command == ReagentQrCommands.ResetScan)
        {
            parameters["parentClass"] = "0x08";
            parameters["subClass"] = "0x05";
            parameters["requestFrameHex"] = Convert.ToHexString(IceImmunoSerialProtocol.BuildRequestFrame(0x08, 0x05));
            parameters["mappingSource"] = "confirmed";
        }
        else
        {
            parameters["mappingSource"] = "mock-until-command-mapping-confirmed";
        }

        return parameters;
    }

    private ReagentQrDeviceOperationResponse ToOperationResponse(
        string commandId,
        string command,
        DeviceCommandResult result,
        bool replayed)
    {
        var statusCode = ReadStatusCode(result);
        return new ReagentQrDeviceOperationResponse(
            result.Ok,
            commandId,
            replayed,
            command,
            result.Status,
            result.ErrorCode,
            result.Message,
            ReadBool(result, "online", result.Ok),
            statusCode,
            ToScanStatus(statusCode),
            ReadString(result, "text"),
            ReadString(result, "position"),
            ReadString(result, "channelCode"),
            deviceAdapter.Mode,
            deviceAdapter.Name);
    }

    private ReagentQrReportResponse ToReportResponse(
        string commandId,
        string command,
        DeviceCommandResult result,
        ResolvedReagentQrText? resolved,
        ReagentScanConfirmationResponse? confirmation,
        bool scanSynced,
        bool replayed,
        string message)
    {
        var statusCode = ReadStatusCode(result);
        return new ReagentQrReportResponse(
            result.Ok && scanSynced,
            commandId,
            replayed,
            command,
            result.Status,
            result.ErrorCode,
            message,
            ReadBool(result, "online", result.Ok),
            statusCode,
            ToScanStatus(statusCode),
            resolved?.Text ?? ReadString(result, "text"),
            resolved?.Position.Code ?? ReadString(result, "position"),
            resolved?.Position.ScannerChannelCode ?? ReadString(result, "channelCode"),
            resolved?.RawBarcode,
            confirmation?.SessionId,
            scanSynced,
            confirmation?.ScanResult,
            confirmation?.ValidationMessage,
            resolved?.ChannelResolvedByMock ?? false,
            deviceAdapter.Mode,
            deviceAdapter.Name);
    }

    private ReagentQrDeviceOperationResponse ToOperationFailure(string? commandId, string command, Exception exception)
    {
        var code = ErrorCode(exception);
        return new ReagentQrDeviceOperationResponse(
            false,
            commandId ?? string.Empty,
            false,
            command,
            DeviceCommandStatuses.Failed,
            code,
            exception.Message,
            false,
            ReagentQrScanStatusCodes.Idle,
            ToScanStatus(ReagentQrScanStatusCodes.Idle),
            null,
            null,
            null,
            deviceAdapter.Mode,
            deviceAdapter.Name);
    }

    private ReagentQrReportResponse ToReportFailure(string? commandId, string command, Exception exception)
    {
        var code = ErrorCode(exception);
        return new ReagentQrReportResponse(
            false,
            commandId ?? string.Empty,
            false,
            command,
            DeviceCommandStatuses.Failed,
            code,
            exception.Message,
            false,
            ReagentQrScanStatusCodes.Idle,
            ToScanStatus(ReagentQrScanStatusCodes.Idle),
            null,
            null,
            null,
            null,
            null,
            false,
            null,
            null,
            false,
            deviceAdapter.Mode,
            deviceAdapter.Name);
    }

    private static string RequireCommandId(string? commandId)
    {
        if (string.IsNullOrWhiteSpace(commandId))
        {
            throw new BusinessRuleException("command_id_required", "commandId is required.", StatusCodes.Status400BadRequest);
        }

        return commandId.Trim();
    }

    private static string ErrorCode(Exception exception) =>
        exception is BusinessRuleException businessRuleException
            ? businessRuleException.Code
            : "reagent_qr_device_operation_failed";

    private static bool ReadBool(DeviceCommandResult result, string key, bool fallback)
    {
        if (!result.Data.TryGetValue(key, out var value) || value is null)
        {
            return fallback;
        }

        return Convert.ToBoolean(value);
    }

    private static ushort ReadStatusCode(DeviceCommandResult result)
    {
        if (!result.Data.TryGetValue("statusCode", out var value) || value is null)
        {
            return ReagentQrScanStatusCodes.Idle;
        }

        return Convert.ToUInt16(value);
    }

    private static string? ReadString(DeviceCommandResult result, string key) =>
        result.Data.TryGetValue(key, out var value) ? Convert.ToString(value) : null;

    private static string ToScanStatus(ushort statusCode) =>
        statusCode == ReagentQrScanStatusCodes.Scanning ? "Scanning" : "Idle";

    private static string KnownReagentName(string code)
    {
        return code switch
        {
            "F01" => "Fixative",
            "F02" => "Blocking reagent",
            "F03" => "Hematoxylin",
            "F04" => "Differentiation solution",
            "F05" => "Alcohol",
            "F06" => "Eosin",
            "F07" => "DAB-A",
            "F08" => "DAB-B",
            "F09" => "Wash solution",
            _ when code.StartsWith('S') => $"Secondary antibody {code}",
            _ when char.IsDigit(code[0]) => $"Primary antibody {code}",
            _ => $"Reagent {code}"
        };
    }

    private static string ReagentType(string code)
    {
        if (code.StartsWith('F'))
        {
            return "auxiliary";
        }

        if (code.StartsWith('S'))
        {
            return "secondary";
        }

        return char.IsDigit(code[0]) ? "primary" : "unknown";
    }

    private sealed record ResolvedPosition(ReagentRackPosition Position, bool ChannelResolvedByMock);

    private sealed record ResolvedReagentQrText(
        string Text,
        string? RawBarcode,
        string? Prefix,
        ReagentRackPosition Position,
        bool ChannelResolvedByMock);
}
