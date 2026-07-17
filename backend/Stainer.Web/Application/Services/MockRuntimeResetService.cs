using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Stainer.Web.Application.ReadModels;
using Stainer.Web.Application.Requests;
using Stainer.Web.Infrastructure.Data;

namespace Stainer.Web.Application.Services;

public sealed class MockRuntimeResetService(
    StainerDbContext dbContext,
    CommandIdempotencyService idempotencyService,
    DeviceModeService deviceModeService)
{
    public Task<CommandResponse> ResetAsync(
        RunCommandRequest request,
        AuthenticatedUser actor,
        CancellationToken cancellationToken = default)
    {
        return idempotencyService.RunAsync(
            request.CommandId,
            "mock_runtime.reset",
            request,
            actor,
            async () =>
            {
                if (!deviceModeService.IsMock)
                {
                    throw new BusinessRuleException(
                        "mock_runtime_reset_mode_required",
                        "Mock runtime reset is allowed only when DeviceMode=Mock.",
                        StatusCodes.Status409Conflict);
                }

                var deleted = 0;
                var updated = 0;

                deleted += await ExecuteDeleteAsync("workflow_step_executions", cancellationToken);
                deleted += await ExecuteDeleteAsync("workflow_executions", cancellationToken);
                deleted += await ExecuteDeleteAsync("dab_batch_tasks", cancellationToken);
                deleted += await ExecuteDeleteAsync("reagent_reservations", cancellationToken);
                deleted += await ExecuteDeleteAsync("reagent_consumptions", cancellationToken);
                deleted += await ExecuteDeleteAsync("system_liquid_usages", cancellationToken);
                deleted += await ExecuteDeleteAsync("dispense_executions", cancellationToken);
                deleted += await ExecuteDeleteAsync("pipetting_operations", cancellationToken);
                deleted += await ExecuteDeleteAsync("device_command_executions", cancellationToken);
                deleted += await ExecuteDeleteAsync("workflow_assignment_history", cancellationToken);
                deleted += await ExecuteDeleteAsync("slide_tasks", cancellationToken);
                deleted += await ExecuteDeleteAsync("sample_scan_items", cancellationToken);
                deleted += await ExecuteDeleteAsync("sample_scan_sessions", cancellationToken);
                deleted += await ExecuteDeleteAsync("staining_tasks", cancellationToken);

                updated += await dbContext.Database.ExecuteSqlRawAsync(
                    """
                    UPDATE channel_batches
                    SET workflow_selection_status = 'Ready',
                        workflow_selection_message = '',
                        selected_workflow_version_id = NULL,
                        workflow_snapshot_json = '',
                        started_at_utc = NULL,
                        completed_at_utc = NULL,
                        updated_at_utc = NULL
                    WHERE status = 'Active'
                    """,
                    cancellationToken);
                deleted += await dbContext.Database.ExecuteSqlRawAsync("DELETE FROM channel_batches WHERE status = 'Active'", cancellationToken);

                updated += await dbContext.Database.ExecuteSqlRawAsync(
                    "UPDATE dab_mix_positions SET active_dab_batch_id = NULL",
                    cancellationToken);
                updated += await dbContext.Database.ExecuteSqlRawAsync(
                    """
                    UPDATE needle_states
                    SET loaded_source_type = 'None',
                        loaded_reagent_code = NULL,
                        source_bottle_id = NULL,
                        dab_batch_id = NULL,
                        system_liquid_source_type = NULL,
                        source_position_code = NULL,
                        volume_ul = 0,
                        needs_wash = 0,
                        current_command_id = NULL,
                        machine_run_id = NULL,
                        workflow_step_execution_id = NULL,
                        device_command_execution_id = NULL,
                        last_error_code = NULL,
                        last_error_message = NULL
                    """,
                    cancellationToken);
                deleted += await ExecuteDeleteAsync("dab_batch_usages", cancellationToken);
                deleted += await ExecuteDeleteAsync("dab_repreparation_plans", cancellationToken);
                deleted += await ExecuteDeleteAsync("dab_batches", cancellationToken);

                deleted += await ExecuteDeleteAsync("reagent_rack_placements", cancellationToken);
                deleted += await ExecuteDeleteAsync("reagent_scan_items", cancellationToken);
                deleted += await ExecuteDeleteAsync("reagent_scan_sessions", cancellationToken);
                deleted += await ExecuteDeleteAsync("reagent_bottles", cancellationToken);

                return new CommandExecutionResult<CommandResponse>(
                    new CommandResponse(
                        true,
                        request.CommandId,
                        false,
                        $"Mock runtime reset completed. Deleted {deleted} rows; updated {updated} rows."),
                    "MockRuntime",
                    "reset");
            },
            cancellationToken);
    }

    private Task<int> ExecuteDeleteAsync(string tableName, CancellationToken cancellationToken)
    {
        var sql = tableName switch
        {
            "workflow_step_executions" => "DELETE FROM workflow_step_executions",
            "workflow_executions" => "DELETE FROM workflow_executions",
            "dab_batch_tasks" => "DELETE FROM dab_batch_tasks",
            "reagent_reservations" => "DELETE FROM reagent_reservations",
            "reagent_consumptions" => "DELETE FROM reagent_consumptions",
            "system_liquid_usages" => "DELETE FROM system_liquid_usages",
            "dispense_executions" => "DELETE FROM dispense_executions",
            "pipetting_operations" => "DELETE FROM pipetting_operations",
            "device_command_executions" => "DELETE FROM device_command_executions",
            "workflow_assignment_history" => "DELETE FROM workflow_assignment_history",
            "slide_tasks" => "DELETE FROM slide_tasks",
            "sample_scan_items" => "DELETE FROM sample_scan_items",
            "sample_scan_sessions" => "DELETE FROM sample_scan_sessions",
            "staining_tasks" => "DELETE FROM staining_tasks",
            "dab_batch_usages" => "DELETE FROM dab_batch_usages",
            "dab_repreparation_plans" => "DELETE FROM dab_repreparation_plans",
            "dab_batches" => "DELETE FROM dab_batches",
            "reagent_rack_placements" => "DELETE FROM reagent_rack_placements",
            "reagent_scan_items" => "DELETE FROM reagent_scan_items",
            "reagent_scan_sessions" => "DELETE FROM reagent_scan_sessions",
            "reagent_bottles" => "DELETE FROM reagent_bottles",
            _ => throw new InvalidOperationException($"Unsupported reset table: {tableName}.")
        };
        return dbContext.Database.ExecuteSqlRawAsync(sql, cancellationToken);
    }
}
