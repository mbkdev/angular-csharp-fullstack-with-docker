using core.Models.Dtos;

namespace core.Services
{
    public interface IAuthenticationService
    {
        Task<string> CreateUserAsync(InputUserDto inboundUser);
        Task<string> CreateAdministratorAsync(InputUserDto inboundUser);
        Task<string> LoginUserAsync(InputLoginUserDto userInfo);
        Task LogoutUserAsync();
        Task<bool> DeleteUserAsync(string userMailAddress);
    }
}
