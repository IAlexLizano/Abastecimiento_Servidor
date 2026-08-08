using MediatR;
using PreOpeningApplication.DTOs;
using PreOpeningApplication.Interfaces;
using Shared.Application.Wrappers;

namespace PreOpeningApplication.Features.Queries
{
    public class GetCkdLotWithDetailsAsyncQuery : IRequest<Response<CkdLotFullDetailsDto>>
    {
        public int CkdLotId { get; set; }
    }

    public class GetCkdLotWithDetailsAsyncQueryHandler : IRequestHandler<GetCkdLotWithDetailsAsyncQuery, Response<CkdLotFullDetailsDto>>
    {
        private readonly IPreOpeningRepository _repository;

        public GetCkdLotWithDetailsAsyncQueryHandler(IPreOpeningRepository repository)
        {
            _repository = repository;
        }

        public async Task<Response<CkdLotFullDetailsDto>> Handle(GetCkdLotWithDetailsAsyncQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _repository.GetCkdLotWithAllDetailsAsync(request.CkdLotId);
                return new Response<CkdLotFullDetailsDto>(result);
            }
            catch (Exception ex)
            {
                return new Response<CkdLotFullDetailsDto> { Message = ex.Message, Succeeded = false };
            }
        }
    }
}
