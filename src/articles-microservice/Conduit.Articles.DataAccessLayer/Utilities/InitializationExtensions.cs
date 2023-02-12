using Conduit.Articles.DataAccessLayer.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Conduit.Articles.DataAccessLayer.Utilities;

public static class InitializationExtensions
{
    public static async Task InitializeDatabase(
        this IServiceScope serviceScope)
    {
        await using var context = serviceScope.ServiceProvider
            .GetRequiredService<ArticlesDbContext>();
        var logger = serviceScope.ServiceProvider
            .GetRequiredService<ILoggerFactory>().CreateLogger(
                "Conduit.Articles.DataAccessLayer.DatabaseInitialization");
        logger.LogInformation("Start database initialization");
        var appliedMigrations =
            await context.Database.GetAppliedMigrationsAsync();
        logger.LogInformation("Applied migrations\n{AppliedMigrations}",
            string.Join('\n', appliedMigrations));
        var pendingMigrations =
            await context.Database.GetPendingMigrationsAsync();
        logger.LogInformation("Pending migrations\n{PendingMigrations}",
            string.Join('\n', pendingMigrations));
        logger.LogInformation("Call ArticlesDbContext.Database.MigrateAsync()");
        await context.Database.MigrateAsync();
        logger.LogInformation("End database initialization");
    }
}
