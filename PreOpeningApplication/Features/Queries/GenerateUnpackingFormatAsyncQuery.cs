using MediatR;
using PreOpeningApplication.DTOs;
using PreOpeningApplication.Interfaces;
using Shared.Application.Wrappers;

namespace PreOpeningApplication.Features.Queries
{
    public class GenerateUnpackingFormatAsyncQuery : IRequest<Response<UnpackingFormatDto>>
    {
        public int CkdLotId { get; set; }
    }

    public class GenerateUnpackingFormatAsyncQueryHandler : IRequestHandler<GenerateUnpackingFormatAsyncQuery, Response<UnpackingFormatDto>>
    {
        private readonly IPreOpeningRepository _repository;

        public GenerateUnpackingFormatAsyncQueryHandler(IPreOpeningRepository repository)
        {
            _repository = repository;
        }

        public async Task<Response<UnpackingFormatDto>> Handle(GenerateUnpackingFormatAsyncQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _repository.GenerateUnpackingFormatAsync(request.CkdLotId);
                return new Response<UnpackingFormatDto>(result);
            }
            catch (Exception ex)
            {
                return new Response<UnpackingFormatDto> { Message = ex.Message, Succeeded = false };
            }
        }
    }
}
