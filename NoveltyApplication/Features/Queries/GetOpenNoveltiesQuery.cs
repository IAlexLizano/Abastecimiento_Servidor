using MediatR;
using NoveltyApplication.DTOs;
using NoveltyApplication.Interfaces;
using Shared.Application.Wrappers;

namespace NoveltyApplication.Features.Queries
{
    public class GetOpenNoveltiesQuery : IRequest<Response<IEnumerable<EngineeringIssueDto>>>
    {
    }

    public class GetOpenNoveltiesQueryHandler : IRequestHandler<GetOpenNoveltiesQuery, Response<IEnumerable<EngineeringIssueDto>>>
    {
        private readonly INoveltyRepository _repository;

        public GetOpenNoveltiesQueryHandler(INoveltyRepository repository)
        {
            _repository = repository;
        }

        public async Task<Response<IEnumerable<EngineeringIssueDto>>> Handle(GetOpenNoveltiesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _repository.GetOpenNovelties();
                return new Response<IEnumerable<EngineeringIssueDto>>(result);
            }
            catch (Exception ex)
            {
                return new Response<IEnumerable<EngineeringIssueDto>> { Message = ex.Message, Succeeded = false };
            }
        }
    }
}
