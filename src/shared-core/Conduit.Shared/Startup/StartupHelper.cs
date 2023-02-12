using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;

namespace Conduit.Shared.Startup;

public static class StartupHelper
{
    private const string CategoryName = "Startup";

    public static async Task WaitHealthyServicesAsync(
        this IServiceScope scope,
        TimeSpan timeout)
    {
        var healthCheckService = scope.ServiceProvider
            .GetRequiredService<HealthCheckService>();
        var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>()
            .CreateLogger(CategoryName);
        var cancellationTokenSource = new CancellationTokenSource(timeout);
        var cancellationToken = cancellationTokenSource.Token;
        try
        {
            var finalReport = await WaitHealthReportAsync(healthCheckService,
                logger, cancellationToken);
            logger.LogInformation(
                "Server healthy: {@Status} {@TotalDuration} {@Entries}",
                finalReport.Status, finalReport.TotalDuration,
                finalReport.Entries);
        }
        catch (OperationCanceledException e)
        {
            throw new StartServerException(e);
        }
    }

    private static async Task<HealthReport> WaitHealthReportAsync(
        HealthCheckService healthCheckService,
        ILogger logger,
        CancellationToken cancellationToken)
    {
        while (true)
        {
            var report =
                await healthCheckService.CheckHealthAsync(cancellationToken);
            if (report.Status == HealthStatus.Healthy)
            {
                return report;
            }

            await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
            logger.LogInformation("Waiting HealthReport");
        }
    }
}
