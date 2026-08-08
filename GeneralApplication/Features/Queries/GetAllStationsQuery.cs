using GeneralApplication.DTOs;
using GeneralApplication.Interfaces;
using MediatR;
using Shared.Application.Wrappers;

namespace GeneralApplication.Features.Queries
{
    public class GetAllStationsQuery : IRequest<Response<IEnumerable<StationDto>>>
    {
    }

    public class GetAllStationsQueryHandler : IRequestHandler<GetAllStationsQuery, Response<IEnumerable<StationDto>>>
    {
        private readonly IStationRepository _repository;

        public GetAllStationsQueryHandler(IStationRepository repository)
        {
            _repository = repository;
        }

        public async Task<Response<IEnumerable<StationDto>>> Handle(GetAllStationsQuery request, CancellationToken cancellationToken)
        {
            var data = await _repository.GetAllStations();
            return new Response<IEnumerable<StationDto>>(data, "Estaciones obtenidas exitosamente.");
        }
    }
}
