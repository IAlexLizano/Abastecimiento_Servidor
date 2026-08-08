using GeneralApplication.DTOs;
using GeneralApplication.Interfaces;
using MediatR;
using Shared.Application.Wrappers;

namespace GeneralApplication.Features.Queries
{
    public class GetAllModelsQuery : IRequest<Response<IEnumerable<ModelDto>>>
    {
    }

    public class GetAllModelsQueryHandler : IRequestHandler<GetAllModelsQuery, Response<IEnumerable<ModelDto>>>
    {
        private readonly IModelRepository _repository;

        public GetAllModelsQueryHandler(IModelRepository repository)
        {
            _repository = repository;
        }

        public async Task<Response<IEnumerable<ModelDto>>> Handle(GetAllModelsQuery request, CancellationToken cancellationToken)
        {
            var data = await _repository.GetAllModels();
            return new Response<IEnumerable<ModelDto>>(data, "Modelos obtenidos exitosamente.");
        }
    }
}
