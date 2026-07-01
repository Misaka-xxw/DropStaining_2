namespace Stainer.Web.Application.Services;

public sealed class DabExpiryHostedService(
    IServiceScopeFactory scopeFactory,
    IConfiguration configuration,
    ILogger<DabExpiryHostedService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var intervalSeconds = Math.Max(5, configuration.GetValue("Dab:ExpiryScanIntervalSeconds", 30));
        using var timer = new PeriodicTimer(TimeSpan.FromSeconds(intervalSeconds));
        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            try
            {
                await using var scope = scopeFactory.CreateAsyncScope();
                var result = await scope.ServiceProvider.GetRequiredService<DabLifecycleService>()
                    .ProcessExpirationsAsync(DateTimeOffset.UtcNow, stoppingToken);
                if (result.NewlyExpiredCount > 0 || result.ReplacementBatchCount > 0)
                {
                    logger.LogInformation(
                        "DAB expiry scan marked {ExpiredCount} batches expired and created {ReplacementCount} replacement batches.",
                        result.NewlyExpiredCount,
                        result.ReplacementBatchCount);
                }
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                return;
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "DAB expiry scan failed; no device command was sent.");
            }
        }
    }
}
