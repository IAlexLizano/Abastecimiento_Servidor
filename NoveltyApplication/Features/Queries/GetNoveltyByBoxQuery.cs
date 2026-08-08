using MediatR;
using NoveltyApplication.DTOs;
using NoveltyApplication.Interfaces;
using Shared.Application.Wrappers;

namespace NoveltyApplication.Features.Queries
{
    public class GetNoveltyByBoxQuery : IRequest<Response<IEnumerable<EngineeringIssueDto>>>
    {
        public int BoxId { get; set; }
    }

    public class GetNoveltyByBoxQueryHandler : IRequestHandler<GetNoveltyByBoxQuery, Response<IEnumerable<EngineeringIssueDto>>>
    {
        private readonly INoveltyRepository _repository;

        public GetNoveltyByBoxQueryHandler(INoveltyRepository repository)
        {
            _repository = repository;
        }

        public async Task<Response<IEnumerable<EngineeringIssueDto>>> Handle(GetNoveltyByBoxQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _repository.GetNoveltyByBoxAsync(request.BoxId);
                return new Response<IEnumerable<EngineeringIssueDto>>(result);
            }
            catch (Exception ex)
            {
                return new Response<IEnumerable<EngineeringIssueDto>> { Message = ex.Message, Succeeded = false };
            }
        }
    }
}
