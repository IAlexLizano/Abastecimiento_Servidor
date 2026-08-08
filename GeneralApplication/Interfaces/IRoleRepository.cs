using GeneralApplication.DTOs;

namespace GeneralApplication.Interfaces
{
    public interface IRoleRepository
    {
        Task<IEnumerable<RoleDto>> GetAllRoles();
    }
}
