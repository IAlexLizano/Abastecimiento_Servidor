using MediatR;
using PreOpeningApplication.DTOs;
using PreOpeningApplication.Interfaces;
using Shared.Application.Wrappers;

namespace PreOpeningApplication.Features.Queries
{
    public class GetAllPalletsByContainerAsyncQuery : IRequest<Response<PalletSimpleListResponseDto>>
    {
        public int IdContainer { get; set; }
    }

    public class GetAllPalletsByContainerAsyncQueryHandler : IRequestHandler<GetAllPalletsByContainerAsyncQuery, Response<PalletSimpleListResponseDto>>
    {
        private readonly IPreOpeningRepository _repository;

        public GetAllPalletsByContainerAsyncQueryHandler(IPreOpeningRepository repository)
        {
            _repository = repository;
        }

        public async Task<Response<PalletSimpleListResponseDto>> Handle(GetAllPalletsByContainerAsyncQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _repository.GetAllPalletsByContainerAsync(request.IdContainer);
                return new Response<PalletSimpleListResponseDto>(result);
            }
            catch (Exception ex)
            {
                return new Response<PalletSimpleListResponseDto> { Message = ex.Message, Succeeded = false };
            }
        }
    }
}
