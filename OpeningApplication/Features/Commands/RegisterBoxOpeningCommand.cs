using AutoMapper;
using MediatR;
using OpeningApplication.DTOs.Opening;
using OpeningApplication.Interfaces;
using Shared.Application.Wrappers;

namespace OpeningApplication.Features.Commands
{
    public class RegisterBoxOpeningCommand : IRequest<Response<BoxOpeningRegisteredResponseDto>>
    {
        public int IdBox { get; set; }
    }

    public class RegisterBoxOpeningCommandHandler : IRequestHandler<RegisterBoxOpeningCommand, Response<BoxOpeningRegisteredResponseDto>>
    {
        private readonly IBoxRepository _repository;
        private readonly IMapper _mapper;

        public RegisterBoxOpeningCommandHandler(IBoxRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response<BoxOpeningRegisteredResponseDto>> Handle(RegisterBoxOpeningCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var requestDto = _mapper.Map<RegisterBoxOpeningRequestDto>(request);
                await _repository.RegisterBoxOpeningAsync(requestDto);
                return new Response<BoxOpeningRegisteredResponseDto>();
            }
            catch (Exception ex)
            {
                return new Response<BoxOpeningRegisteredResponseDto> { Message = ex.Message, Succeeded = false };
            }
        }
    }
}
