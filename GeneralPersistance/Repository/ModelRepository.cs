using Microsoft.EntityFrameworkCore;
using Shared.Application.Exceptions;
using Domains.Entities;
using GeneralApplication.Interfaces;
using GeneralPersistance.Context;

namespace GeneralPersistance.Repository
{
    public class ModelRepository : IModelRepository
    {
        private readonly ApplicationContext _dbContext;

        public ModelRepository(ApplicationContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<IEnumerable<GeneralApplication.DTOs.ModelDto>> GetAllModels()
        {
            return await _dbContext.Models
                .Select(m => new GeneralApplication.DTOs.ModelDto
                {
                    IdModel = m.IdModel,
                    ModelCode = m.ModelCode,
                    ModelName = m.ModelName
                })
                .ToListAsync();
        }
    }
}
