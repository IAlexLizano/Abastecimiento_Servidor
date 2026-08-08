using AutoMapper;
using ComponentsApplication.DTOs;
using ComponentsApplication.Interfaces;
using MediatR;
using Shared.Application.Wrappers;

namespace ComponentsApplication.Features.Commands
{
    public class LoadRecipeCommand : IRequest<Response<string>>
    {
        public string ModelCode { get; set; } = null!;
        public List<LoadRecipeRequestDto> Recipes { get; set; } = new List<LoadRecipeRequestDto>();
    }

    public class LoadRecipeCommandHandler : IRequestHandler<LoadRecipeCommand, Response<string>>
    {
        private readonly IComponentsRepository _repository;

        public LoadRecipeCommandHandler(IComponentsRepository repository)
        {
            _repository = repository;
        }

        public async Task<Response<string>> Handle(LoadRecipeCommand request, CancellationToken cancellationToken)
        {
            var result = await _repository.LoadRecipe(request.ModelCode, request.Recipes);
            return new Response<string>(result, result);
        }
    }
}
