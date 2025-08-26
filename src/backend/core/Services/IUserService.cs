using core.Models;

namespace core.Services
{
    public interface IUserService
    {
        Task<UserModel> GetCurrentUserProfileAsync(string? userId);
    }
}
