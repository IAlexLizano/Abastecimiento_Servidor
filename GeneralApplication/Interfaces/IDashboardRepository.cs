using System.Threading.Tasks;
using GeneralApplication.DTOs.Dashboard;

namespace GeneralApplication.Interfaces
{
    public interface IDashboardRepository
    {
        Task<DashboardKpisResponseDto> GetDashboardKpisAsync(DashboardFilterDto filter);
    }
}
