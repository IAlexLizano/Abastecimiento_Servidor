using AuthApplication.DTOs;
using AuthApplication.Features.Commands;
using AutoMapper;

namespace AuthApplication.Mappings
{
   public class GeneralMapping : Profile
    {
        public GeneralMapping() {
            CreateMap<LoginCommand, LoginRequestDto>();
        }
    }
}
