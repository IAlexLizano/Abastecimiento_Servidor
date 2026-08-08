using ComponentsApplication.DTOs;
using ComponentsApplication.Interfaces;
using MediatR;
using Shared.Application.Wrappers;

namespace ComponentsApplication.Features.Queries
{
    public class GetComponentByIdQuery : IRequest<Response<ComponentResponseDto>>
    {
        public int ComponentId { get; set; }
    }

    public class GetComponentByIdQueryHandler : IRequestHandler<GetComponentByIdQuery, Response<ComponentResponseDto>>
    {
        private readonly IComponentsRepository _repository;

        public GetComponentByIdQueryHandler(IComponentsRepository repository)
        {
            _repository = repository;
        }

        public async Task<Response<ComponentResponseDto>> Handle(GetComponentByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _repository.GetComponentById(request.ComponentId);
            return new Response<ComponentResponseDto>(result);

        }
    }
}
