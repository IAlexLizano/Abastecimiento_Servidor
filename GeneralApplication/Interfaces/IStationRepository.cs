using GeneralApplication.DTOs;

namespace GeneralApplication.Interfaces
{
    public interface IStationRepository
    {
        Task<IEnumerable<StationDto>> GetAllStations();
    }
}
