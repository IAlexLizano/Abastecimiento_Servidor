using GeneralApplication.DTOs;
using GeneralApplication.Interfaces;
using MediatR;
using Shared.Application.Wrappers;

namespace GeneralApplication.Features.Queries
{
    public class GetAllStoresQuery : IRequest<Response<IEnumerable<StoreDto>>>
    {
    }

    public class GetAllStoresQueryHandler : IRequestHandler<GetAllStoresQuery, Response<IEnumerable<StoreDto>>>
    {
        private readonly IStoreRepository _repository;

        public GetAllStoresQueryHandler(IStoreRepository repository)
        {
            _repository = repository;
        }

        public async Task<Response<IEnumerable<StoreDto>>> Handle(GetAllStoresQuery request, CancellationToken cancellationToken)
        {
            var data = await _repository.GetAllStores();
            return new Response<IEnumerable<StoreDto>>(data, "Almacenes obtenidos exitosamente.");
        }
    }
}
