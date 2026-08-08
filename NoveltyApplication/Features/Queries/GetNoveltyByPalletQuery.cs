using MediatR;
using NoveltyApplication.DTOs;
using NoveltyApplication.Interfaces;
using Shared.Application.Wrappers;

namespace NoveltyApplication.Features.Queries
{
    public class GetNoveltyByPalletQuery : IRequest<Response<IEnumerable<EngineeringIssueDto>>>
    {
        public int PalletId { get; set; }
    }

    public class GetNoveltyByPalletQueryHandler : IRequestHandler<GetNoveltyByPalletQuery, Response<IEnumerable<EngineeringIssueDto>>>
    {
        private readonly INoveltyRepository _repository;

        public GetNoveltyByPalletQueryHandler(INoveltyRepository repository)
        {
            _repository = repository;
        }

        public async Task<Response<IEnumerable<EngineeringIssueDto>>> Handle(GetNoveltyByPalletQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _repository.GetNoveltyByPalletAsync(request.PalletId);
                return new Response<IEnumerable<EngineeringIssueDto>>(result);
            }
            catch (Exception ex)
            {
                return new Response<IEnumerable<EngineeringIssueDto>> { Message = ex.Message, Succeeded = false };
            }
        }
    }
}
