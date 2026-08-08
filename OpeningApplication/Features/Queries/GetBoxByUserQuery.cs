using MediatR;
using OpeningApplication.DTOs.Opening;
using OpeningApplication.Interfaces;
using Shared.Application.Wrappers;

namespace OpeningApplication.Features.Queries
{
    public class GetBoxByUserQuery : IRequest<Response<BoxWithLabeledBoxesResponseDto>>
    {
        public int UserId { get; set; }
    }

    public class GetBoxByUserQueryHandler : IRequestHandler<GetBoxByUserQuery, Response<BoxWithLabeledBoxesResponseDto>>
    {
        private readonly IBoxRepository _repository;

        public GetBoxByUserQueryHandler(IBoxRepository repository)
        {
            _repository = repository;
        }

        public async Task<Response<BoxWithLabeledBoxesResponseDto>> Handle(GetBoxByUserQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _repository.GetBoxesByUserAsync(request.UserId);
                return new Response<BoxWithLabeledBoxesResponseDto>(result);
            }
            catch (Exception ex)
            {
                return new Response<BoxWithLabeledBoxesResponseDto> { Message = ex.Message, Succeeded = false };
            }
        }
    }
}
