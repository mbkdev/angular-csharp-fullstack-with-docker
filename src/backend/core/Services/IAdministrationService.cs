using core.Models;

namespace core.Services
{
    public interface IAdministrationService
    {
        Task<IEnumerable<OutputUsersWithRolesDto>> GetAllUsersWithRolesAsync();
    }
}
