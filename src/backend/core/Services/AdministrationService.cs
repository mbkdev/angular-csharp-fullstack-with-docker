using core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace core.Services
{
    public class AdministrationService(ILogger<AdministrationService> logger, UserManager<IdentityUser> userManager) : IAdministrationService
    {
        private readonly ILogger<AdministrationService> logger = logger;
        private readonly UserManager<IdentityUser> userManager = userManager;

        public async Task<IEnumerable<OutputUsersWithRolesDto>> GetAllUsersWithRolesAsync()
        {
            var allUsers = new List<OutputUsersWithRolesDto>();

            var users = await this.userManager.Users.ToListAsync();

            if (users.Count != 0)
            {
                foreach (var user in users)
                {
                    var roles = await this.userManager.GetRolesAsync(user);

                    var userWithRoles = new OutputUsersWithRolesDto
                    {
                        Email = user.Email!,
                        Username = user.UserName!,
                        Roles = [.. roles]
                    };


                    allUsers.Add(userWithRoles);
                }
            }

            return allUsers;
        }
    }
}
