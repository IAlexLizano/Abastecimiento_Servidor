using AutoMapper;
using ComponentsApplication.DTOs;
using ComponentsApplication.Interfaces;
using MediatR;
using Shared.Application.Wrappers;

namespace NoveltyApplication.Features.Commands
{
    public class CreateComponentCommand : IRequest<Response<string>>
    {
        public string PartCode { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string? TechParameters { get; set; }

        public string? ImagePath { get; set; }
    }

    public class CreateComponentCommandHandler : IRequestHandler<CreateComponentCommand, Response<string>>
    {
        private readonly IMapper _mapper;
        private readonly IComponentsRepository _repository;

        public CreateComponentCommandHandler(IMapper mapper, IComponentsRepository repository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _repository = repository;
        }

        public async Task<Response<string>> Handle(CreateComponentCommand request, CancellationToken cancellationToken)
        {
            var createComponentDto = _mapper.Map<CreateComponentRequestDto>(request);
            var result = await _repository.CreateComponent(createComponentDto);
            return new Response<string>(result, result);
        }
    }
}
