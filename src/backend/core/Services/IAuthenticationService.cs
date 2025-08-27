using core.Models.Dtos;

namespace core.Services
{
    public interface IAuthenticationService
    {
        Task<string> CreateUserAsync(InputUserDto inboundUser);
        Task<string> CreateAdministratorAsync(InputUserDto inboundUser);
        Task<bool> DeleteUserAsync(string userMailAddress);
        Task<string> LoginUserAsync(InputLoginUserDto userInfo);
        Task LogoutUserAsync();
    }
}
