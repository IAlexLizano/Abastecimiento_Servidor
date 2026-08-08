using Microsoft.EntityFrameworkCore;
using Shared.Application.Exceptions;
using Domains.Entities;
using GeneralApplication.Interfaces;
using GeneralPersistance.Context;

namespace GeneralPersistance.Repository
{
    public class StationRepository : IStationRepository
    {
        private readonly ApplicationContext _dbContext;

        public StationRepository(ApplicationContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<IEnumerable<GeneralApplication.DTOs.StationDto>> GetAllStations()
        {
            return await _dbContext.WorkStation
                .Select(s => new GeneralApplication.DTOs.StationDto
                {
                    IdStation = s.IdStation,
                    CodeStation = s.CodeStation,
                    Name = s.Name
                })
                .ToListAsync();
        }
    }
}
