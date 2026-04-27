using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CourseWork.Services;

public class AlertBackgroundService(
    IServiceScopeFactory scopeFactory,
    ILogger<AlertBackgroundService> logger) : BackgroundService
{
    private static readonly TimeSpan Interval = TimeSpan.FromMinutes(30);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("AlertBackgroundService запущено.");

        // Перша генерація відразу після старту
        await RunGeneration();

        using var timer = new PeriodicTimer(Interval);
        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            await RunGeneration();
        }
    }

    private async Task RunGeneration()
    {
        try
        {
            using var scope = scopeFactory.CreateScope();
            var alertService = scope.ServiceProvider.GetRequiredService<ISystemAlertService>();
            await alertService.GenerateAutomaticAlertsAsync();
            logger.LogInformation("Автоматичні сповіщення згенеровано ({Time}).", DateTime.UtcNow);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Помилка при генерації автоматичних сповіщень.");
        }
    }
}