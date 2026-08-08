using AutoMapper;
using OpeningApplication.DTOs.Opening;
using OpeningApplication.DTOs.Teams;
using OpeningApplication.Features.Commands;

namespace OpeningApplication.Mappings
{
   public class GeneralMapping : Profile
    {
        public GeneralMapping() {
            CreateMap<AssignTeamToBoxCommand, AssignTeamToBoxRequestDto>();
            CreateMap<RegisterUnboxingCommand, RegisterUnboxingRequestDto>();
            CreateMap<RegisterBoxOpeningCommand, RegisterBoxOpeningRequestDto>();
        }
    }
}
