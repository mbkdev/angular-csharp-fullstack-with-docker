using core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace core.Services
{
    public class AdministrationService : IAdministrationService
    {
        private readonly ILogger<AdministrationService> logger;
        private readonly UserManager<IdentityUser> userManager;

        public AdministrationService(ILogger<AdministrationService> logger, UserManager<IdentityUser> userManager)
        {
            this.logger = logger;
            this.userManager = userManager;
        }

        public async Task<IEnumerable<OutputUsersWithRolesDto>> GetAllUsersWithRolesAsync()
        {
            var allUsers = new List<OutputUsersWithRolesDto>();

            var users = await userManager.Users.ToListAsync();

            foreach (var user in users)
            {
                var roles = await userManager.GetRolesAsync(user);

                var userWithRoles = new OutputUsersWithRolesDto
                {
                    Email = user.Email,
                    Username = user.UserName,
                    Roles = [.. roles]
                };


                allUsers.Add(userWithRoles);
            }

            return allUsers;
        }
    }
}
