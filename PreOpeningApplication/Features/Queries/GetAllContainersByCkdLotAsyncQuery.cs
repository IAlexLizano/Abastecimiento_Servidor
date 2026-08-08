using MediatR;
using PreOpeningApplication.DTOs;
using PreOpeningApplication.Interfaces;
using Shared.Application.Wrappers;

namespace PreOpeningApplication.Features.Queries
{
    public class GetAllContainersByCkdLotAsyncQuery : IRequest<Response<ContainerSimpleListResponseDto>>
    {
        public int IdCkdLot { get; set; }
    }

    public class GetAllContainersByCkdLotAsyncQueryHandler : IRequestHandler<GetAllContainersByCkdLotAsyncQuery, Response<ContainerSimpleListResponseDto>>
    {
        private readonly IPreOpeningRepository _repository;

        public GetAllContainersByCkdLotAsyncQueryHandler(IPreOpeningRepository repository)
        {
            _repository = repository;
        }

        public async Task<Response<ContainerSimpleListResponseDto>> Handle(GetAllContainersByCkdLotAsyncQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _repository.GetAllContainersByCkdLotAsync(request.IdCkdLot);
                return new Response<ContainerSimpleListResponseDto>(result);
            }
            catch (Exception ex)
            {
                return new Response<ContainerSimpleListResponseDto> { Message = ex.Message, Succeeded = false };
            }
        }
    }
}
