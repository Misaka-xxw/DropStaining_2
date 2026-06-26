using System.Collections.Concurrent;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text;

namespace Stainer.Web.Application.Services;

public sealed class SafetyLogWriter(IConfiguration configuration, IHostEnvironment environment)
{
    private static readonly ConcurrentDictionary<string, SemaphoreSlim> FileLocks = new(StringComparer.OrdinalIgnoreCase);

    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

    private readonly string logDirectory = ResolveLogDirectory(configuration, environment);

    public async Task WriteAsync(
        string category,
        string level,
        string message,
        SafetyLogContext context,
        Exception? exception = null,
        CancellationToken cancellationToken = default)
    {
        Directory.CreateDirectory(logDirectory);
        var safeCategory = NormalizeCategory(category);
        var path = Path.Combine(logDirectory, $"{safeCategory}-{DateTimeOffset.UtcNow:yyyyMMdd}.jsonl");
        var entry = new
        {
            occurredAtUtc = DateTimeOffset.UtcNow,
            category = safeCategory,
            level,
            message = Scrub(message),
            correlationId = context.CorrelationId,
            commandId = context.CommandId,
            machineRunId = context.MachineRunId,
            channelBatchId = context.ChannelBatchId,
            slideTaskId = context.SlideTaskId,
            workflowStepExecutionId = context.WorkflowStepExecutionId,
            deviceCommandExecutionId = context.DeviceCommandExecutionId,
            deviceMode = context.DeviceMode,
            actor = context.Actor,
            source = context.Source,
            exception = exception is null ? null : Scrub(exception.Message)
        };
        var line = JsonSerializer.Serialize(entry, JsonOptions) + Environment.NewLine;
        var bytes = Encoding.UTF8.GetBytes(line);
        var fileLock = FileLocks.GetOrAdd(path, _ => new SemaphoreSlim(1, 1));
        await fileLock.WaitAsync(cancellationToken);
        try
        {
            await using var stream = new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.ReadWrite, 4096, useAsync: true);
            await stream.WriteAsync(bytes, cancellationToken);
        }
        finally
        {
            fileLock.Release();
        }
    }

    public string LogDirectory => logDirectory;

    public static string Scrub(string? value)
    {
        var text = value ?? string.Empty;
        if (text.Contains("password", StringComparison.OrdinalIgnoreCase)
            || text.Contains("token", StringComparison.OrdinalIgnoreCase)
            || text.Contains("connectionString", StringComparison.OrdinalIgnoreCase)
            || text.Contains("connection string", StringComparison.OrdinalIgnoreCase)
            || text.Contains("Data Source=", StringComparison.OrdinalIgnoreCase))
        {
            return "[redacted sensitive details]";
        }

        return text;
    }

    private static string ResolveLogDirectory(IConfiguration configuration, IHostEnvironment environment)
    {
        var configured = configuration["Safety:LogDirectory"] ?? configuration["Logging:SafetyLogDirectory"];
        if (!string.IsNullOrWhiteSpace(configured))
        {
            return Path.IsPathRooted(configured)
                ? configured
                : Path.GetFullPath(Path.Combine(environment.ContentRootPath, configured));
        }

        return Path.GetFullPath(Path.Combine(environment.ContentRootPath, "..", "..", "data", "logs"));
    }

    private static string NormalizeCategory(string value)
    {
        var chars = value.Select(ch => char.IsLetterOrDigit(ch) ? char.ToLowerInvariant(ch) : '-').ToArray();
        var normalized = new string(chars).Trim('-');
        return string.IsNullOrWhiteSpace(normalized) ? "runtime" : normalized;
    }
}

public sealed record SafetyLogContext(
    string? CorrelationId = null,
    string? CommandId = null,
    string? MachineRunId = null,
    string? ChannelBatchId = null,
    string? SlideTaskId = null,
    string? WorkflowStepExecutionId = null,
    string? DeviceCommandExecutionId = null,
    string DeviceMode = "Mock",
    string? Actor = null,
    string Source = "system");
