using MediatR;
using NoveltyApplication.DTOs;
using NoveltyApplication.Interfaces;
using Shared.Application.Wrappers;

namespace NoveltyApplication.Features.Queries
{
    public class GetNoveltyByIdQuery : IRequest<Response<EngineeringIssueDto>>
    {
        public int IssueId { get; set; }
    }

    public class GetNoveltyByIdQueryHandler : IRequestHandler<GetNoveltyByIdQuery, Response<EngineeringIssueDto>>
    {
        private readonly INoveltyRepository _repository;

        public GetNoveltyByIdQueryHandler(INoveltyRepository repository)
        {
            _repository = repository;
        }

        public async Task<Response<EngineeringIssueDto>> Handle(GetNoveltyByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _repository.GetNoveltyByIdAsync(request.IssueId);
                return new Response<EngineeringIssueDto>(result);
            }
            catch (Exception ex)
            {
                return new Response<EngineeringIssueDto> { Message = ex.Message, Succeeded = false };
            }
        }
    }
}
