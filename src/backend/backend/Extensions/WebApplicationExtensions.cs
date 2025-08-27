using core.Models;
using core.Services;
using data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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

        public static async Task CreateFirstRunData(this WebApplication webApplication, ILogger logger, ConfigurationManager configuration)
        {
            var initialConfiguration = configuration.GetSection("FirstStart").Get<FirstStartModel>();
            if (initialConfiguration == null)
            {
                var errorMessage = @"Could not found the section ""FirstStart"" with the key ""InitialAdministratorMail"" and ""InitialAdministratorPassword""";

                logger.LogError(errorMessage);
                throw new Exception(errorMessage);
            }

            var initialAdministratorEmail = initialConfiguration.InitialAdministratorMail;
            var initialAdministratorPassword = initialConfiguration.InitialAdministratorPassword;

            if (initialAdministratorEmail.IsNullOrEmpty() || initialAdministratorPassword.IsNullOrEmpty())
            {
                var errorMessage = $@"One of the properties (""{nameof(initialAdministratorEmail)}""  or ""{nameof(initialAdministratorPassword)}"") is empty. Both must be set.";
                logger.LogError(errorMessage, initialAdministratorEmail, initialAdministratorPassword);

                throw new Exception(errorMessage);
            }

            await using var serviceScope = webApplication.Services.CreateAsyncScope();
            await using var dbContext = serviceScope.ServiceProvider.GetRequiredService<BackendDbContext>();

            if (!dbContext.Users.Any())
            {
                var authenticationService = serviceScope.ServiceProvider.GetRequiredService<IAuthenticationService>();
                var token = await authenticationService.CreateAdministratorAsync(new core.Models.Dtos.InputUserDto
                {
                    Email = initialAdministratorEmail,
                    Password = initialAdministratorPassword
                });

                logger.LogInformation(token);
            }
        }
    }
}
