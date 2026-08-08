using ComponentsApplication.DTOs;
using ComponentsApplication.Interfaces;
using MediatR;
using Shared.Application.Wrappers;

namespace ComponentsApplication.Features.Queries
{
    public class GetRecipesByModelQuery : IRequest<Response<IEnumerable<RecipeResponseDto>>>
    {
        public int IdModel { get; set; }
    }

    public class GetRecipesByModelQueryHandler : IRequestHandler<GetRecipesByModelQuery, Response<IEnumerable<RecipeResponseDto>>>
    {
        private readonly IComponentsRepository _repository;

        public GetRecipesByModelQueryHandler(IComponentsRepository repository)
        {
            _repository = repository;
        }

        public async Task<Response<IEnumerable<RecipeResponseDto>>> Handle(GetRecipesByModelQuery request, CancellationToken cancellationToken)
        {
            var recipes = await _repository.GetRecipesByModel(request.IdModel);
            return new Response<IEnumerable<RecipeResponseDto>>(recipes, "Recetas obtenidas con éxito.");
        }
    }
}
