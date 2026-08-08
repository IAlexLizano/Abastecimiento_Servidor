using Microsoft.EntityFrameworkCore;
using Shared.Application.Exceptions;
using Domains.Entities;
using GeneralApplication.Interfaces;
using GeneralPersistance.Context;

namespace GeneralPersistance.Repository
{
    public class RoleRepository : IRoleRepository
    {
        private readonly ApplicationContext _dbContext;

        public RoleRepository(ApplicationContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<IEnumerable<GeneralApplication.DTOs.RoleDto>> GetAllRoles()
        {
            return await _dbContext.Roles
                .Select(r => new GeneralApplication.DTOs.RoleDto
                {
                    IdRole = r.IdRole,
                    Name = r.Name,
                    IsActive = r.IsActive
                })
                .ToListAsync();
        }
    }
}
