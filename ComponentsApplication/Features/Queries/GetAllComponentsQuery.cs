using ComponentsApplication.DTOs;
using ComponentsApplication.Interfaces;
using MediatR;
using Shared.Application.Wrappers;

namespace ComponentsApplication.Features.Queries
{
    public class GetAllComponentsQuery : IRequest<Response<IEnumerable<ComponentResponseDto>>>
    {
    }

    public class GetAllComponentsQueryHandler : IRequestHandler<GetAllComponentsQuery, Response<IEnumerable<ComponentResponseDto>>>
    {
        private readonly IComponentsRepository _repository;

        public GetAllComponentsQueryHandler(IComponentsRepository repository)
        {
            _repository = repository;
        }

        public async Task<Response<IEnumerable<ComponentResponseDto>>> Handle(GetAllComponentsQuery request, CancellationToken cancellationToken)
        {
                var result = await _repository.GetAllComponents();
                return new Response<IEnumerable<ComponentResponseDto>>(result);
        }
    }
}
