using AuthApplication.DTOs.Stores;
using AuthApplication.Interfaces;
using MediatR;
using Shared.Application.Wrappers;

namespace AuthApplication.Features.Queries.Stores
{
    /// <summary>
    /// Query para obtener todas las tiendas
    /// </summary>
    public class GetAllStoresQuery : IRequest<Response<List<StoreResponseDto>>>
    {
    }

    /// <summary>
    /// Handler para la query de obtener todas las tiendas
    /// </summary>
    public class GetAllStoresQueryHandler : IRequestHandler<GetAllStoresQuery, Response<List<StoreResponseDto>>>
    {
        private readonly IStoresRepository _storesRepository;

        public GetAllStoresQueryHandler(IStoresRepository storesRepository)
        {
            _storesRepository = storesRepository ?? throw new ArgumentNullException(nameof(storesRepository));
        }

        public async Task<Response<List<StoreResponseDto>>> Handle(GetAllStoresQuery request, CancellationToken cancellationToken)
        {
            var response = await _storesRepository.GetAllStoresAsync();
            return new Response<List<StoreResponseDto>>(response, "Bodegas obtenidas correctamente");
        }
    }
}
