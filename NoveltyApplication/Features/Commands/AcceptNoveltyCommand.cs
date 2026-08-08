using MediatR;
using NoveltyApplication.Interfaces;
using Shared.Application.Wrappers;

namespace NoveltyApplication.Features.Commands
{
    public class AcceptNoveltyCommand : IRequest<Response<Unit>>
    {
        public int IssueId { get; set; }
    }

    public class AcceptNoveltyCommandHandler : IRequestHandler<AcceptNoveltyCommand, Response<Unit>>
    {
        private readonly INoveltyRepository _repository;

        public AcceptNoveltyCommandHandler(INoveltyRepository repository)
        {
            _repository = repository;
        }

        public async Task<Response<Unit>> Handle(AcceptNoveltyCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _repository.AcceptNoveltyAsync(request.IssueId);
                return new Response<Unit>(Unit.Value);
            }
            catch (Exception ex)
            {
                return new Response<Unit> { Message = ex.Message, Succeeded = false };
            }
        }
    }
}
