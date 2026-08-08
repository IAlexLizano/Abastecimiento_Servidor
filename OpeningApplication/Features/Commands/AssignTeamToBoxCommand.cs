using AutoMapper;
using MediatR;
using OpeningApplication.DTOs.Teams;
using OpeningApplication.Interfaces;
using Shared.Application.Wrappers;

namespace OpeningApplication.Features.Commands
{
    public class AssignTeamToBoxCommand : IRequest<Response<string>>
    {
        public int BoxId { get; set; }
        public IEnumerable<TeamMemberDto> TeamMembers { get; set; }
    }

    public class AssignTeamToBoxCommandHandler : IRequestHandler<AssignTeamToBoxCommand, Response<string>>
    {
        private readonly ITeamRepository _repository;
        private readonly IMapper _mapper;

        public AssignTeamToBoxCommandHandler(ITeamRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response<string>> Handle(AssignTeamToBoxCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var requestDto = _mapper.Map<AssignTeamToBoxRequestDto>(request);
                var result = await _repository.AssignTeamToBoxAsync(requestDto);
                return new Response<string>(result, "Equipo asignado exitosamente");
            }
            catch (Exception ex)
            {
                return new Response<string> { Message = ex.Message, Succeeded = false };
            }
        }
    }
}
