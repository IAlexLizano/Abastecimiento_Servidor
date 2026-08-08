using Identity.Context;
using AuthApplication.Interfaces;
using AuthApplication.DTOs.Stores;
using Domains.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.Application.Exceptions;

namespace Identity.Repository
{
    /// <summary>
    /// Servicio para gestión de tiendas
    /// </summary>
    public class StoresRepository : IStoresRepository
    {
        private readonly ApplicationContext _dbContext;

        public StoresRepository(
            ApplicationContext dbContext
        )
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<List<StoreResponseDto>> GetAllStoresAsync()
        {
            try
            {
                var stores = await _dbContext.Store
                    .AsNoTracking()
                    .OrderBy(s => s.StoreName)
                    .Select(s => new StoreResponseDto
                    {
                        IdStore = s.IdStore,
                        StoreName = s.StoreName,
                        StoreCode = s.StoreCode
                    })
                    .ToListAsync();

                return stores;
            }
            catch (DbUpdateException ex)
            {
                throw new ApiException($"Error al obtener bodegas: {ex.Message}");
            }
        }
    }
}
