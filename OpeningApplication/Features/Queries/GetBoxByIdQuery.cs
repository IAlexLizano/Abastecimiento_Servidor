using MediatR;
using OpeningApplication.DTOs.Opening;
using OpeningApplication.Interfaces;
using Shared.Application.Wrappers;

namespace OpeningApplication.Features.Queries
{
    public class GetBoxByIdQuery : IRequest<Response<BoxWithLabeledBoxesResponseDto>>
    {
        public int BoxId { get; set; }
    }

    public class GetBoxByIdQueryHandler : IRequestHandler<GetBoxByIdQuery, Response<BoxWithLabeledBoxesResponseDto>>
    {
        private readonly IBoxRepository _repository;

        public GetBoxByIdQueryHandler(IBoxRepository repository)
        {
            _repository = repository;
        }

        public async Task<Response<BoxWithLabeledBoxesResponseDto>> Handle(GetBoxByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _repository.GetBoxAsync(request.BoxId);
                return new Response<BoxWithLabeledBoxesResponseDto>(result);
            }
            catch (Exception ex)
            {
                return new Response<BoxWithLabeledBoxesResponseDto> { Message = ex.Message, Succeeded = false };
            }
        }
    }
}
