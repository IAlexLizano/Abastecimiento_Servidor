using MediatR;
using Shared.Application.Wrappers;
using SupplyingApplication.DTOs;
using SupplyingApplication.Interfaces;

namespace SupplyingApplication.Features.Queries
{
    /// <summary>
    /// Query para obtener cajas en estado EN_PROCESO
    /// </summary>
    public class GetSuppliesInProgressQuery : IRequest<Response<BoxListResponseDto>>
    {
    }

    /// <summary>
    /// Handler para obtener supplies en proceso
    /// </summary>
    public class GetSuppliesInProgressQueryHandler : IRequestHandler<GetSuppliesInProgressQuery, Response<BoxListResponseDto>>
    {
        private readonly ISupplyingRepository _repository;

        public GetSuppliesInProgressQueryHandler(ISupplyingRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<Response<BoxListResponseDto>> Handle(GetSuppliesInProgressQuery request, CancellationToken cancellationToken)
        {
            var result = await _repository.GetSuppliesInProgressAsync();
            return new Response<BoxListResponseDto>(result, "Cajas en proceso obtenidas exitosamente");
        }
    }
}
