using MediatR;
using NoveltyApplication.Interfaces;
using Shared.Application.Wrappers;

namespace NoveltyApplication.Features.Commands
{
    public class ResolveNoveltyCommand : IRequest<Response<Unit>>
    {
        public int IssueId { get; set; }
    }

    public class ResolveNoveltyCommandHandler : IRequestHandler<ResolveNoveltyCommand, Response<Unit>>
    {
        private readonly INoveltyRepository _repository;

        public ResolveNoveltyCommandHandler(INoveltyRepository repository)
        {
            _repository = repository;
        }

        public async Task<Response<Unit>> Handle(ResolveNoveltyCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _repository.ResolveNoveltyAsync(request.IssueId);
                return new Response<Unit>(Unit.Value);
            }
            catch (Exception ex)
            {
                return new Response<Unit> { Message = ex.Message, Succeeded = false };
            }
        }
    }
}
