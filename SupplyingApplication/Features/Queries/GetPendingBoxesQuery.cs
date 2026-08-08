using MediatR;
using Shared.Application.Wrappers;
using SupplyingApplication.DTOs;
using SupplyingApplication.Interfaces;

namespace SupplyingApplication.Features.Queries
{
    /// <summary>
    /// Query para obtener cartones pendientes de abastecimiento
    /// </summary>
    public class GetPendingBoxesQuery : IRequest<Response<BoxListResponseDto>>
    {
    }

    /// <summary>
    /// Handler para obtener cartones pendientes
    /// </summary>
    public class GetPendingBoxesQueryHandler : IRequestHandler<GetPendingBoxesQuery, Response<BoxListResponseDto>>
    {
        private readonly ISupplyingRepository _repository;

        public GetPendingBoxesQueryHandler(ISupplyingRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<Response<BoxListResponseDto>> Handle(GetPendingBoxesQuery request, CancellationToken cancellationToken)
        {
            var result = await _repository.GetPendingBoxesAsync();
            return new Response<BoxListResponseDto>(result, "Cartones pendientes obtenidos exitosamente");
        }
    }
}
