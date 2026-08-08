using MediatR;
using OpeningApplication.DTOs.Opening;
using OpeningApplication.DTOs.Teams;
using OpeningApplication.Interfaces;
using Shared.Application.Wrappers;

namespace OpeningApplication.Features.Queries
{
    public class GetTeamAssignmentByBoxQuery : IRequest<Response<TeamByBoxResponseDto>>
    {
        public int BoxId { get; set; }
    }

    public class GetTeamAssignmentByBoxQueryHandler : IRequestHandler<GetTeamAssignmentByBoxQuery, Response<TeamByBoxResponseDto>>
    {
        private readonly ITeamRepository _repository;

        public GetTeamAssignmentByBoxQueryHandler(ITeamRepository repository)
        {
            _repository = repository;
        }

        public async Task<Response<TeamByBoxResponseDto>> Handle(GetTeamAssignmentByBoxQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _repository.GetTeamAssignmentByBoxAsync(request.BoxId);
                return new Response<TeamByBoxResponseDto>(result);
            }
            catch (Exception ex)
            {
                return new Response<TeamByBoxResponseDto> { Message = ex.Message, Succeeded = false };
            }
        }
    }
}
