using MediatR;
using NoveltyApplication.DTOs;
using NoveltyApplication.Interfaces;
using Shared.Application.Wrappers;

namespace NoveltyApplication.Features.Queries
{
    public class GetNoveltyByUserQuery : IRequest<Response<IEnumerable<EngineeringIssueDto>>>
    {
        public int UserId { get; set; }
    }

    public class GetNoveltyByUserQueryHandler : IRequestHandler<GetNoveltyByUserQuery, Response<IEnumerable<EngineeringIssueDto>>>
    {
        private readonly INoveltyRepository _repository;

        public GetNoveltyByUserQueryHandler(INoveltyRepository repository)
        {
            _repository = repository;
        }

        public async Task<Response<IEnumerable<EngineeringIssueDto>>> Handle(GetNoveltyByUserQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _repository.GetNoveltyByUserAsync(request.UserId);
                return new Response<IEnumerable<EngineeringIssueDto>>(result);
            }
            catch (Exception ex)
            {
                return new Response<IEnumerable<EngineeringIssueDto>> { Message = ex.Message, Succeeded = false };
            }
        }
    }
}
