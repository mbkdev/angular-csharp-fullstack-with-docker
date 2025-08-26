using core.Exceptions;
using core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace core.Services
{
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> logger;
        private readonly UserManager<IdentityUser> userManager;

        public UserService(ILogger<UserService> logger, UserManager<IdentityUser> userManager)
        {
            this.logger = logger;
            this.userManager = userManager;
        }

        public async Task<UserModel> GetCurrentUserProfileAsync(string? userId)
        {
            if (userId is null)
            {
                throw new ArgumentNullException();
            }

            var user = await this.userManager.FindByIdAsync(userId);
            if (user is null)
            {
                throw new UserNotFoundException(userId);
            }

            return new UserModel
            {
                Email = user.Email,
                Username = user.UserName
            };
        }
    }
}
