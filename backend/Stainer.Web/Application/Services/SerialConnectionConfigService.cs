using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Stainer.Web.Application.ReadModels;
using Stainer.Web.Application.Requests;
using Stainer.Web.Domain.Entities;
using Stainer.Web.Infrastructure.Data;

namespace Stainer.Web.Application.Services;

// 串口连接配置服务：按 deviceKey 读取 / upsert 持久化。镜像 ScannerConfigurationService 的幂等 + 审计模式。
// 仅持久化配置，不打开真实串口。
public sealed class SerialConnectionConfigService(
    StainerDbContext dbContext,
    CommandIdempotencyService idempotencyService)
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
    private static readonly string[] AllowedParity = { SerialParityNames.None, SerialParityNames.Odd, SerialParityNames.Even, SerialParityNames.Mark, SerialParityNames.Space };
    private static readonly string[] AllowedStopBits = { SerialStopBitsNames.One, SerialStopBitsNames.OnePointFive, SerialStopBitsNames.Two };
    private static readonly string[] AllowedHandshake = { SerialHandshakeNames.None };

    public async Task<SerialConnectionResponse> GetAsync(string deviceKey, CancellationToken cancellationToken = default)
    {
        var key = NormalizeDeviceKey(deviceKey);
        var profile = await dbContext.SerialConnectionProfiles
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.DeviceKey == key, cancellationToken);
        return ToResponse(profile ?? CreateDefault(key));
    }

    public Task<SerialConnectionMutationResponse> SaveAsync(
        string deviceKey,
        SaveSerialConnectionRequest request,
        AuthenticatedUser actor,
        CancellationToken cancellationToken = default)
    {
        deviceKey = NormalizeDeviceKey(deviceKey);
        return idempotencyService.RunAsync(
            request.CommandId,
            "serial_connection.save",
            new { deviceKey, request },
            actor,
            async () =>
            {
                RequireReason(request.Reason);
                var port = NormalizeOptionalValue(request.PortName, "portName", 128);
                var baud = ValidatePositive(request.BaudRate, "baudRate", 1_000_000) ?? 115200;
                var dataBits = ValidateDataBits(request.DataBits);
                var parity = RequireChoice(request.Parity, "parity", AllowedParity, SerialParityNames.None);
                var stopBits = RequireChoice(request.StopBits, "stopBits", AllowedStopBits, SerialStopBitsNames.One);
                var handshake = RequireChoice(request.Handshake, "handshake", AllowedHandshake, SerialHandshakeNames.None);
                var readTimeout = ValidatePositive(request.ReadTimeoutMilliseconds, "readTimeoutMilliseconds", 600_000) ?? 2000;
                var writeTimeout = ValidatePositive(request.WriteTimeoutMilliseconds, "writeTimeoutMilliseconds", 600_000) ?? 2000;

                var profile = await dbContext.SerialConnectionProfiles
                    .SingleOrDefaultAsync(x => x.DeviceKey == deviceKey, cancellationToken);
                object? before = profile is null ? null : ToAudit(profile);
                if (profile is null)
                {
                    profile = new SerialConnectionProfile
                    {
                        DeviceKey = deviceKey,
                        CreatedAtUtc = DateTimeOffset.UtcNow
                    };
                    dbContext.SerialConnectionProfiles.Add(profile);
                }

                profile.PortName = port;
                profile.BaudRate = baud;
                profile.DataBits = dataBits;
                profile.Parity = parity;
                profile.StopBits = stopBits;
                profile.Handshake = handshake;
                profile.ReadTimeoutMilliseconds = readTimeout;
                profile.WriteTimeoutMilliseconds = writeTimeout;
                profile.Enabled = true;
                profile.UpdatedAtUtc = DateTimeOffset.UtcNow;

                AddAudit(actor, "serial_connection.save", "SerialConnectionProfile", profile.Id, before, ToAudit(profile), request.Reason);

                return new CommandExecutionResult<SerialConnectionMutationResponse>(
                    new SerialConnectionMutationResponse(true, request.CommandId, false, "SerialConnectionProfile", profile.Id, "Serial connection config saved."),
                    "SerialConnectionProfile",
                    profile.Id);
            },
            cancellationToken);
    }

    private static SerialConnectionProfile CreateDefault(string deviceKey) => new()
    {
        DeviceKey = deviceKey,
        PortName = null,
        BaudRate = 115200,
        DataBits = 8,
        Parity = SerialParityNames.None,
        StopBits = SerialStopBitsNames.One,
        Handshake = SerialHandshakeNames.None,
        ReadTimeoutMilliseconds = 2000,
        WriteTimeoutMilliseconds = 2000,
        Enabled = true
    };

    private static SerialConnectionResponse ToResponse(SerialConnectionProfile p) => new(
        p.DeviceKey, p.PortName, p.BaudRate, p.DataBits, p.Parity, p.StopBits, p.Handshake,
        p.ReadTimeoutMilliseconds, p.WriteTimeoutMilliseconds, p.Enabled, p.CreatedAtUtc, p.UpdatedAtUtc);

    private static object ToAudit(SerialConnectionProfile p) => new
    {
        p.DeviceKey, p.PortName, p.BaudRate, p.DataBits, p.Parity, p.StopBits, p.Handshake,
        p.ReadTimeoutMilliseconds, p.WriteTimeoutMilliseconds, p.Enabled
    };

    private void AddAudit(AuthenticatedUser actor, string action, string entityType, string entityId, object? before, object after, string reason)
    {
        dbContext.AuditLogs.Add(new AuditLog
        {
            ActorUserId = actor.UserId,
            Action = action,
            EntityType = entityType,
            EntityId = entityId,
            Message = JsonSerializer.Serialize(new { before, after, reason = reason.Trim(), actor = actor.Username }, JsonOptions),
            CreatedAtUtc = DateTimeOffset.UtcNow
        });
    }

    private static string NormalizeDeviceKey(string? deviceKey)
    {
        var normalized = deviceKey?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(normalized))
        {
            return SerialConnectionDeviceKeys.MainController;
        }

        if (normalized.Length > 64)
        {
            throw new BusinessRuleException("device_key_too_long", "deviceKey must be at most 64 characters.", StatusCodes.Status400BadRequest);
        }

        return normalized;
    }

    private static int? ValidateDataBits(int? value)
    {
        if (!value.HasValue)
        {
            return 8;
        }

        if (value is < 5 or > 8)
        {
            throw new BusinessRuleException("data_bits_invalid", "dataBits must be between 5 and 8.", StatusCodes.Status400BadRequest);
        }

        return value;
    }

    private static int? ValidatePositive(int? value, string fieldName, int maximum)
    {
        if (!value.HasValue)
        {
            return null;
        }

        if (value <= 0 || value > maximum)
        {
            throw new BusinessRuleException($"{ToCode(fieldName)}_invalid", $"{fieldName} must be between 1 and {maximum}.", StatusCodes.Status400BadRequest);
        }

        return value;
    }

    private static string RequireChoice(string? value, string fieldName, string[] allowed, string defaultValue)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return defaultValue;
        }

        var normalized = value.Trim();
        return Array.Exists(allowed, x => string.Equals(x, normalized, StringComparison.OrdinalIgnoreCase))
            ? normalized
            : throw new BusinessRuleException($"{ToCode(fieldName)}_invalid", $"{fieldName} must be one of: {string.Join(", ", allowed)}.", StatusCodes.Status400BadRequest);
    }

    private static string? NormalizeOptionalValue(string? value, string fieldName, int maxLength)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        var normalized = value.Trim();
        if (normalized.Length > maxLength)
        {
            throw new BusinessRuleException($"{ToCode(fieldName)}_too_long", $"{fieldName} must be at most {maxLength} characters.", StatusCodes.Status400BadRequest);
        }

        return normalized;
    }

    private static void RequireReason(string? reason)
    {
        var normalized = reason?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(normalized))
        {
            throw new BusinessRuleException("reason_required", "reason is required.", StatusCodes.Status400BadRequest);
        }

        if (normalized.Length > 2000)
        {
            throw new BusinessRuleException("reason_too_long", "reason must be at most 2000 characters.", StatusCodes.Status400BadRequest);
        }
    }

    private static string ToCode(string fieldName)
    {
        return string.Concat(fieldName.Select((ch, index) =>
            char.IsUpper(ch) && index > 0 ? $"_{char.ToLowerInvariant(ch)}" : char.ToLowerInvariant(ch).ToString()));
    }
}
