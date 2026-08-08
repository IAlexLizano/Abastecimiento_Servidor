using AuthApplication.DTOs;
using AuthApplication.DTOs.Users;
using AuthApplication.Features.Commands;
using AuthApplication.Features.Commands.Users;
using AutoMapper;

namespace AuthApplication.Mappings
{
   public class GeneralMapping : Profile
    {
        public GeneralMapping() {
            CreateMap<LoginCommand, LoginRequestDto>();

            // User mappings
            CreateMap<CreateUserCommand, CreateUserRequestDto>();
            CreateMap<UpdateUserCommand, UpdateUserRequestDto>();
        }
    }
}
