using MediatR;
using OpeningApplication.DTOs.Opening;
using OpeningApplication.Interfaces;
using Shared.Application.Wrappers;

namespace OpeningApplication.Features.Queries
{
    public class GetBoxesByPalletQuery : IRequest<Response<IEnumerable<BoxWithLabeledBoxesResponseDto>>>
    {
        public int PalletId { get; set; }
    }

    public class GetBoxesByPalletWithDetailsQueryHandler : IRequestHandler<GetBoxesByPalletQuery, Response<IEnumerable<BoxWithLabeledBoxesResponseDto>>>
    {
        private readonly IBoxRepository _repository;

        public GetBoxesByPalletWithDetailsQueryHandler(IBoxRepository repository)
        {
            _repository = repository;
        }

        public async Task<Response<IEnumerable<BoxWithLabeledBoxesResponseDto>>> Handle(GetBoxesByPalletQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _repository.GetBoxesByPalletAsync(request.PalletId);
                return new Response<IEnumerable<BoxWithLabeledBoxesResponseDto>>(result);
            }
            catch (Exception ex)
            {
                return new Response<IEnumerable<BoxWithLabeledBoxesResponseDto>> { Message = ex.Message, Succeeded = false };
            }
        }
    }
}
