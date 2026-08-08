using Microsoft.EntityFrameworkCore;
using Shared.Application.Exceptions;
using Domains.Entities;
using GeneralApplication.Interfaces;
using GeneralPersistance.Context;

namespace GeneralPersistance.Repository
{
    public class StoreRepository : IStoreRepository
    {
        private readonly ApplicationContext _dbContext;

        public StoreRepository(ApplicationContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<IEnumerable<GeneralApplication.DTOs.StoreDto>> GetAllStores()
        {
            return await _dbContext.Store
                .Select(s => new GeneralApplication.DTOs.StoreDto
                {
                    IdStore = s.IdStore,
                    StoreCode = s.StoreCode,
                    StoreName = s.StoreName
                })
                .ToListAsync();
        }
    }
}
