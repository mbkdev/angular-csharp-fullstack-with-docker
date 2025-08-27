using core.Exceptions;
using core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace core.Services
{
    public class UserService(ILogger<UserService> logger, UserManager<IdentityUser> userManager) : IUserService
    {
        private readonly ILogger<UserService> logger = logger;
        private readonly UserManager<IdentityUser> userManager = userManager;

        public async Task<UserModel> GetCurrentUserProfileAsync(string? userId)
        {
            ArgumentNullException.ThrowIfNull(userId);

            var user = await this.userManager.FindByIdAsync(userId);

            if (user is null)
            {
                throw new UserNotFoundException(userId);
            }

            return new UserModel
            {
                Email = user.Email!,
                Username = user.UserName!
            };
        }
    }
}
