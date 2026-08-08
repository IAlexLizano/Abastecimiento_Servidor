using AutoMapper;
using ComponentsApplication.DTOs;
using ComponentsApplication.Interfaces;
using MediatR;
using Shared.Application.Wrappers;

namespace ComponentsApplication.Features.Commands
{
    public class UpdateComponentCommand : IRequest<Response<string>>
    {
        public int ComponentId { get; set; }
        public string PartCode { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string? TechParameters { get; set; }

        public string? ImagePath { get; set; }
    }

    public class UpdateComponentCommandHandler : IRequestHandler<UpdateComponentCommand, Response<string>>
    {
        private readonly IMapper _mapper;
        private readonly IComponentsRepository _repository;

        public UpdateComponentCommandHandler(IMapper mapper, IComponentsRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<Response<string>> Handle(UpdateComponentCommand request, CancellationToken cancellationToken)
        {
            var updateComponentDto = _mapper.Map<UpdateComponentRequestDto>(request);
            var response = await _repository.UpdateComponent(updateComponentDto);
            return new Response<string>(response, response);
        }
    }
}
