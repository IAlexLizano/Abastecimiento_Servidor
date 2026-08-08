using AutoMapper;
using MediatR;
using PreOpeningApplication.DTOs;
using PreOpeningApplication.Interfaces;
using Shared.Application.Wrappers;

namespace PreOpeningApplication.Features.Commands
{
    public class StorePalletAsyncCommand : IRequest<Response<StorePalletResponseDto>>
    {
        public int IdPallet { get; set; }
        public string? Content { get; set; }
        public string? Description { get; set; }
        public bool? Claim { get; set; }
    }

    public class StorePalletAsyncCommandHandler : IRequestHandler<StorePalletAsyncCommand, Response<StorePalletResponseDto>>
    {
        private readonly IPreOpeningRepository _repository;
        private readonly IMapper _mapper;

        public StorePalletAsyncCommandHandler(IPreOpeningRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response<StorePalletResponseDto>> Handle(StorePalletAsyncCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var requestDto = _mapper.Map<StorePalletRequestDto>(request);
                var result = await _repository.StorePalletAsync(requestDto);
                return new Response<StorePalletResponseDto>(result);
            }
            catch (Exception ex)
            {
                return new Response<StorePalletResponseDto> { Message = ex.Message, Succeeded = false };
            }
        }
    }
}
