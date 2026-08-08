using AutoMapper;
using MediatR;
using PreOpeningApplication.DTOs;
using PreOpeningApplication.Interfaces;
using Shared.Application.Wrappers;

namespace PreOpeningApplication.Features.Commands
{
    public class StoreContainerAsyncCommand : IRequest<Response<StoreContainerResponseDto>>
    {
        public int IdContainer { get; set; }
        public int IdStore { get; set; }
    }

    public class StoreContainerAsyncCommandHandler : IRequestHandler<StoreContainerAsyncCommand, Response<StoreContainerResponseDto>>
    {
        private readonly IPreOpeningRepository _repository;
        private readonly IMapper _mapper;
        public StoreContainerAsyncCommandHandler(IPreOpeningRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response<StoreContainerResponseDto>> Handle(StoreContainerAsyncCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var requestDto = _mapper.Map<StoreContainerAsyncCommand, StoreContainerRequestDto>(request);
                var result = await _repository.StoreContainerAsync(requestDto);
                return new Response<StoreContainerResponseDto>(result);
            }
            catch (Exception ex)
            {
                return new Response<StoreContainerResponseDto> { Message = ex.Message, Succeeded = false };
            }
        }
    }
}
