using MediatR;
using Shared.Application.Wrappers;
using SupplyingApplication.DTOs;
using SupplyingApplication.Interfaces;

namespace SupplyingApplication.Features.Queries
{
    /// <summary>
    /// Query para obtener todos los cartones sin importar el estado
    /// </summary>
    public class GetAllBoxesQuery : IRequest<Response<BoxListResponseDto>>
    {
    }

    /// <summary>
    /// Handler para obtener todos los cartones
    /// </summary>
    public class GetAllBoxesQueryHandler : IRequestHandler<GetAllBoxesQuery, Response<BoxListResponseDto>>
    {
        private readonly ISupplyingRepository _repository;

        public GetAllBoxesQueryHandler(ISupplyingRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<Response<BoxListResponseDto>> Handle(GetAllBoxesQuery request, CancellationToken cancellationToken)
        {
            var result = await _repository.GetAllBoxesAsync();
            return new Response<BoxListResponseDto>(result, "Todos los cartones obtenidos exitosamente");
        }
    }
}
