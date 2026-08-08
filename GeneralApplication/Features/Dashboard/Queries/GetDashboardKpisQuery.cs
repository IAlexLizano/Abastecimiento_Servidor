using GeneralApplication.DTOs.Dashboard;
using GeneralApplication.Interfaces;
using MediatR;
using Shared.Application.Wrappers;

namespace GeneralApplication.Features.Dashboard.Queries
{
    public class GetDashboardKpisQuery : IRequest<Response<DashboardKpisResponseDto>>
    {
        public DashboardFilterDto Filter { get; set; } = new DashboardFilterDto();
    }

    public class GetDashboardKpisQueryHandler : IRequestHandler<GetDashboardKpisQuery, Response<DashboardKpisResponseDto>>
    {
        private readonly IDashboardRepository _dashboardRepository;

        public GetDashboardKpisQueryHandler(IDashboardRepository dashboardRepository)
        {
            _dashboardRepository = dashboardRepository;
        }

        public async Task<Response<DashboardKpisResponseDto>> Handle(GetDashboardKpisQuery request, CancellationToken cancellationToken)
        {
            var result = await _dashboardRepository.GetDashboardKpisAsync(request.Filter);
            return new Response<DashboardKpisResponseDto>(result);
        }
    }
}
