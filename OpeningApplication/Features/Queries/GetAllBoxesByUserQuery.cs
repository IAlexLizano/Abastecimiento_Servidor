using MediatR;
using OpeningApplication.DTOs.Opening;
using OpeningApplication.Interfaces;
using Shared.Application.Wrappers;

namespace OpeningApplication.Features.Queries
{
    public class GetAllBoxesByUserQuery : IRequest<Response<IEnumerable<BoxWithLabeledBoxesResponseDto>>>
    {
        public int UserId { get; set; }
    }

    public class GetAllBoxesByUserQueryHandler : IRequestHandler<GetAllBoxesByUserQuery, Response<IEnumerable<BoxWithLabeledBoxesResponseDto>>>
    {
        private readonly IBoxRepository _repository;

        public GetAllBoxesByUserQueryHandler(IBoxRepository repository)
        {
            _repository = repository;
        }

        public async Task<Response<IEnumerable<BoxWithLabeledBoxesResponseDto>>> Handle(GetAllBoxesByUserQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _repository.GetAllBoxesByUserAsync(request.UserId);
                return new Response<IEnumerable<BoxWithLabeledBoxesResponseDto>>(result);
            }
            catch (Exception ex)
            {
                return new Response<IEnumerable<BoxWithLabeledBoxesResponseDto>> { Message = ex.Message, Succeeded = false };
            }
        }
    }
}
