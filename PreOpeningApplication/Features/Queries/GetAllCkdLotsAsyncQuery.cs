using MediatR;
using PreOpeningApplication.DTOs;
using PreOpeningApplication.Interfaces;
using Shared.Application.Wrappers;

namespace PreOpeningApplication.Features.Queries
{
    public class GetAllCkdLotsAsyncQuery : IRequest<Response<CkdLotSimpleListResponseDto>>
    {
        public string? Status { get; set; }
    }

    public class GetAllCkdLotsAsyncQueryHandler : IRequestHandler<GetAllCkdLotsAsyncQuery, Response<CkdLotSimpleListResponseDto>>
    {
        private readonly IPreOpeningRepository _repository;

        public GetAllCkdLotsAsyncQueryHandler(IPreOpeningRepository repository)
        {
            _repository = repository;
        }

        public async Task<Response<CkdLotSimpleListResponseDto>> Handle(GetAllCkdLotsAsyncQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _repository.GetAllCkdLotsAsync(request.Status);
                return new Response<CkdLotSimpleListResponseDto>(result);
            }
            catch (Exception ex)
            {
                return new Response<CkdLotSimpleListResponseDto> { Message = ex.Message, Succeeded = false };
            }
        }
    }
}
