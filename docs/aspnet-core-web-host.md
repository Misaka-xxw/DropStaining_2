# ASP.NET Core Web Host Startup

This project now uses `backend/Stainer.Web` as the local ASP.NET Core web host for the kiosk UI.

## Start

From the repository root:

```powershell
dotnet run --project backend\Stainer.Web\Stainer.Web.csproj
```

The default local URL is:

```text
http://127.0.0.1:5205
```

Open the browser in full-screen kiosk mode and navigate to:

```text
http://127.0.0.1:5205/
```

## Stop

In the terminal running the service, press:

```text
Ctrl+C
```

If the process was started in the background, stop the matching `dotnet` process from Task Manager or with PowerShell after confirming the process id:

```powershell
Get-Process dotnet
Stop-Process -Id <pid>
```

## Boundaries

- The C# ASP.NET Core service is the formal local web host.
- The old FastAPI/Jinja source under `src/app` was removed in step 6 (it had existed only as a prototype reference); the formal web UI is now solely `/control-console`.
- Jinja is not used by the ASP.NET Core host.
- Business state is read from application services and SQLite. Mock mode is limited to simulated device behavior behind `IDeviceAdapter`.
- The default binding is local-only (`127.0.0.1`), not LAN-facing.

## Device adapter and initialization

- `Device:Mode=Mock` selects `MockDeviceAdapter` through dependency injection.
- `Device:Mode=Real` selects `UnavailableRealDeviceAdapter` until a real adapter is implemented. It returns `real_adapter_not_implemented` and never falls back to Mock or sends a hardware command.
- `MachineExecutor` calls `IDeviceAdapter`; reagent consumption, DAB usage, alarms, audit, and workflow execution remain formal database business operations.
- Mock fault plans are controlled through engineer/admin APIs and carry a reason and `commandId`.
- Initialization runs and their individual checks are persisted in `device_initialization_runs` and `device_initialization_checks`.

Formal initialization endpoints:

```text
GET  /api/device/state
GET  /api/device-initialization
POST /api/device-initialization
POST /api/device-initialization/{initializationRunId}/retry
POST /api/device/mock-faults
POST /api/device/mock-faults/clear
```

Run start and preflight require a successful initialization for the current device mode.

## Local runtime files

The ASP.NET Core host creates the following local runtime files when needed:

- Structured safety logs: `data/logs/*.jsonl`
- MachineExecutor lease: `data/machine-executor.lock`
- Database backups: `data/backups/`
- Development database: `data/stainer.db`

These files are machine-local operational state. Logs, the executor lease, and
database backups are ignored by Git and must not be committed. The development
database is also ignored by the existing `*.db` rule. EF Core migrations and
intentional reference-data seed code remain version controlled.

Verify the repository policy from the workspace root:

```powershell
powershell -ExecutionPolicy Bypass -File tools\verify-runtime-git-policy.ps1
```
