using MediatR;
using PreOpeningApplication.DTOs;
using PreOpeningApplication.Interfaces;
using Shared.Application.Wrappers;

namespace PreOpeningApplication.Features.Queries
{
    public class GenerateContainerUnloadFormatAsyncQuery : IRequest<Response<ContainerUnloadingFormatDto>>
    {
        public int CkdLotId { get; set; }
    }

    public class GenerateContainerUnloadFormatAsyncQueryHandler : IRequestHandler<GenerateContainerUnloadFormatAsyncQuery, Response<ContainerUnloadingFormatDto>>
    {
        private readonly IPreOpeningRepository _repository;

        public GenerateContainerUnloadFormatAsyncQueryHandler(IPreOpeningRepository repository)
        {
            _repository = repository;
        }

        public async Task<Response<ContainerUnloadingFormatDto>> Handle(GenerateContainerUnloadFormatAsyncQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _repository.GenerateContainerUnloadingFormatAsync(request.CkdLotId);
                return new Response<ContainerUnloadingFormatDto>(result);
            }
            catch (Exception ex)
            {
                return new Response<ContainerUnloadingFormatDto> { Message = ex.Message, Succeeded = false };
            }
        }
    }
}
