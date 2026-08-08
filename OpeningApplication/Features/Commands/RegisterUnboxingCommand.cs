using AutoMapper;
using MediatR;
using OpeningApplication.DTOs.Opening;
using OpeningApplication.Interfaces;
using Shared.Application.Wrappers;

namespace OpeningApplication.Features.Commands
{
    public class RegisterUnboxingCommand : IRequest<Response<Unit>>
    {
        public int IdLabeledBox { get; set; }
        public string? Description { get; set; }
    }

    public class RegisterUnboxingCommandHandler : IRequestHandler<RegisterUnboxingCommand, Response<Unit>>
    {
        private readonly IBoxRepository _repository;
        private readonly IMapper _mapper;
        public RegisterUnboxingCommandHandler(IBoxRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response<Unit>> Handle(RegisterUnboxingCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var requestDto = _mapper.Map<RegisterUnboxingRequestDto>(request);
                await _repository.RegisterUnboxingAsync(requestDto);
                return new Response<Unit>(Unit.Value);
            }
            catch (Exception ex)
            {
                return new Response<Unit> { Message = ex.Message, Succeeded = false };
            }
        }
    }
}
