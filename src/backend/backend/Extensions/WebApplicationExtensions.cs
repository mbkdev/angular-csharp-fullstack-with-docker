using data;
using Microsoft.EntityFrameworkCore;

namespace backend.Extensions
{
    public static class WebApplicationExtensions
    {
        public static async Task CheckDatabaseActuatlity(this WebApplication webApplication, ILogger logger)
        {
            logger.LogInformation("Check if database is up to date...");

            await using (var serviceScope = webApplication.Services.CreateAsyncScope())
            await using (var dbContext = serviceScope.ServiceProvider.GetRequiredService<BackendDbContext>())
            {
                var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync();
                var pendingMigrationCount = pendingMigrations.Count();

                if (pendingMigrationCount > 0)
                {
                    logger.LogWarning($"Found {pendingMigrationCount} pending migrations. Trying to migrate...");

                    await dbContext.Database.MigrateAsync();

                    logger.LogInformation("Database-migration was successful.");

                    return;
                }
            }

            logger.LogInformation("Database is up to date.");
        }
    }
}
