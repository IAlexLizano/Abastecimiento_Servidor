using AuthApplication.DTOs.Roles;
using Shared.Application.Wrappers;

namespace AuthApplication.Interfaces
{
    public interface IRolesRepository
    {
        /// <summary>
        /// Obtiene todos los roles disponibles
        /// </summary>
        /// <returns>Lista de roles con Response wrapper</returns>
        Task<List<RoleResponseDto>> GetAllRolesAsync();
    }
}
