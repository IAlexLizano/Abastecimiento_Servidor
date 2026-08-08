using MediatR;
using PreOpeningApplication.DTOs;
using PreOpeningApplication.Interfaces;
using Shared.Application.Wrappers;

namespace PreOpeningApplication.Features.Queries
{
    public class GetPalletsByContainerAsyncQuery : IRequest<Response<PalletsByContainerResponseDto>>
    {
        public int ContainerId { get; set; }
    }

    public class GetPalletsByContainerAsyncQueryHandler : IRequestHandler<GetPalletsByContainerAsyncQuery, Response<PalletsByContainerResponseDto>>
    {
        private readonly IPreOpeningRepository _repository;

        public GetPalletsByContainerAsyncQueryHandler(IPreOpeningRepository repository)
        {
            _repository = repository;
        }

        public async Task<Response<PalletsByContainerResponseDto>> Handle(GetPalletsByContainerAsyncQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _repository.GetPalletsByContainerAsync(request.ContainerId);
                return new Response<PalletsByContainerResponseDto>(result);
            }
            catch (Exception ex)
            {
                return new Response<PalletsByContainerResponseDto> { Message = ex.Message, Succeeded = false };
            }
        }
    }
}
