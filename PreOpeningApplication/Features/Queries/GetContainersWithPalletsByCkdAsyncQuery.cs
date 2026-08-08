using MediatR;
using PreOpeningApplication.DTOs;
using PreOpeningApplication.Interfaces;
using Shared.Application.Wrappers;

namespace PreOpeningApplication.Features.Queries
{
    public class GetContainersWithPalletsByCkdAsyncQuery : IRequest<Response<ContainersWithPalletsByCkdResponseDto>>
    {
        public int CkdLotId { get; set; }
    }

    public class GetContainersWithPalletsByCkdAsyncQueryHandler : IRequestHandler<GetContainersWithPalletsByCkdAsyncQuery, Response<ContainersWithPalletsByCkdResponseDto>>
    {
        private readonly IPreOpeningRepository _repository;

        public GetContainersWithPalletsByCkdAsyncQueryHandler(IPreOpeningRepository repository)
        {
            _repository = repository;
        }

        public async Task<Response<ContainersWithPalletsByCkdResponseDto>> Handle(GetContainersWithPalletsByCkdAsyncQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _repository.GetContainersWithPalletsByCkdAsync(request.CkdLotId);
                return new Response<ContainersWithPalletsByCkdResponseDto>(result);
            }
            catch (Exception ex)
            {
                return new Response<ContainersWithPalletsByCkdResponseDto> { Message = ex.Message, Succeeded = false };
            }
        }
    }
}
