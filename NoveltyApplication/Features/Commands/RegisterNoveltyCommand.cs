using MediatR;
using NoveltyApplication.DTOs;
using NoveltyApplication.Interfaces;
using Shared.Application.Wrappers;

namespace NoveltyApplication.Features.Commands
{
    public class RegisterNoveltyCommand : IRequest<Response<EngineeringIssueDto>>
    {
        public int BoxId { get; set; }
        public int ComponentId { get; set; }
        public int Quantity { get; set; }
        public string Reason { get; set; } = null!;
    }

    public class RegisterNoveltyCommandHandler : IRequestHandler<RegisterNoveltyCommand, Response<EngineeringIssueDto>>
    {
        private readonly INoveltyRepository _repository;

        public RegisterNoveltyCommandHandler(INoveltyRepository repository)
        {
            _repository = repository;
        }

        public async Task<Response<EngineeringIssueDto>> Handle(RegisterNoveltyCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var requestDto = new RegisterNoveltyRequestDto 
                { 
                    BoxId = request.BoxId, 
                    ComponentId = request.ComponentId, 
                    Quantity = request.Quantity, 
                    UserId = 1, 
                    Reason = request.Reason 
                };
                var result = await _repository.RegisterNoveltyAsync(requestDto);
                return new Response<EngineeringIssueDto>(result);
            }
            catch (Exception ex)
            {
                return new Response<EngineeringIssueDto> { Message = ex.Message, Succeeded = false };
            }
        }
    }
}
