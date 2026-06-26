using System.Diagnostics;
using Microsoft.Data.Sqlite;
using System.Net;
using Stainer.Web.Application.ReadModels;

namespace Stainer.Web.Application.Services;

public sealed class MachineExecutorLeaseService : IDisposable
{
    private FileStream? leaseStream;
    private string? failureReason;

    public MachineExecutorLeaseService(IConfiguration configuration, IHostEnvironment environment)
        : this(ResolveLockPath(configuration, environment))
    {
    }

    public MachineExecutorLeaseService(string lockPath)
    {
        LockPath = lockPath;
        OwnerId = $"{Dns.GetHostName()}:{Environment.ProcessId}:{Guid.NewGuid():N}";
    }

    public string LockPath { get; }

    public string OwnerId { get; }

    public bool IsOwner => leaseStream is not null;

    public bool ReadOnlyMode => !IsOwner;

    public bool TryAcquire()
    {
        if (leaseStream is not null)
        {
            return true;
        }

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(LockPath) ?? ".");
            leaseStream = new FileStream(LockPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
            leaseStream.SetLength(0);
            using var writer = new StreamWriter(leaseStream, leaveOpen: true);
            writer.WriteLine(OwnerId);
            writer.WriteLine(Process.GetCurrentProcess().StartTime.ToUniversalTime().ToString("O"));
            writer.Flush();
            leaseStream.Flush(true);
            failureReason = null;
            return true;
        }
        catch (IOException ex)
        {
            failureReason = ex.Message;
            return false;
        }
        catch (UnauthorizedAccessException ex)
        {
            failureReason = ex.Message;
            return false;
        }
    }

    public void EnsureOwner()
    {
        if (!IsOwner)
        {
            throw new BusinessRuleException("executor_lease_unavailable", "This backend instance is not the MachineExecutor owner.", StatusCodes.Status409Conflict);
        }
    }

    public ExecutorLeaseStatusResponse GetStatus()
    {
        return new ExecutorLeaseStatusResponse(IsOwner, ReadOnlyMode, OwnerId, LockPath, failureReason);
    }

    public void Release()
    {
        leaseStream?.Dispose();
        leaseStream = null;
    }

    public void Dispose()
    {
        Release();
    }

    private static string ResolveLockPath(IConfiguration configuration, IHostEnvironment environment)
    {
        var configured = configuration["MachineExecutor:LeasePath"];
        if (!string.IsNullOrWhiteSpace(configured))
        {
            return Path.IsPathRooted(configured)
                ? configured
                : Path.GetFullPath(Path.Combine(environment.ContentRootPath, configured));
        }

        if (environment.IsEnvironment("Testing"))
        {
            var connectionString = configuration.GetConnectionString("StainerDatabase") ?? configuration["ConnectionStrings:StainerDatabase"];
            if (!string.IsNullOrWhiteSpace(connectionString))
            {
                var builder = new SqliteConnectionStringBuilder(connectionString);
                if (!string.IsNullOrWhiteSpace(builder.DataSource))
                {
                    var databasePath = Path.IsPathRooted(builder.DataSource)
                        ? builder.DataSource
                        : Path.GetFullPath(Path.Combine(environment.ContentRootPath, builder.DataSource));
                    return Path.Combine(Path.GetDirectoryName(databasePath) ?? environment.ContentRootPath, "machine-executor.lock");
                }
            }
        }

        return Path.GetFullPath(Path.Combine(environment.ContentRootPath, "..", "..", "data", "machine-executor.lock"));
    }
}
