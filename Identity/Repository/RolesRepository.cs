using Identity.Context;
using AuthApplication.Interfaces;
using AuthApplication.DTOs.Roles;
using Domains.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.Application.Exceptions;
using Shared.Application.Wrappers;

namespace Identity.Repository
{
    /// <summary>
    /// Servicio para gestión de roles
    /// </summary>
    public class RolesRepository : IRolesRepository
    {
        private readonly ApplicationContext _dbContext;

        public RolesRepository(
            ApplicationContext dbContext
        )
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<List<RoleResponseDto>> GetAllRolesAsync()
        {
            try
            {
                var roles = await _dbContext.Role
                    .AsNoTracking()
                    .OrderBy(r => r.Name)
                    .Select(r => new RoleResponseDto
                    {
                        IdRole = r.IdRole,
                        Name = r.Name,
                        IsActive = r.IsActive
                    })
                    .ToListAsync();

                return roles;
            }
            catch (DbUpdateException ex)
            {
                throw new ApiException($"Error al obtener los roles: {ex.Message}");
            }
        }
    }
}
