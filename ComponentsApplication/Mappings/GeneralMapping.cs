using AutoMapper;
using ComponentsApplication.DTOs;
using ComponentsApplication.Features.Commands;
using NoveltyApplication.Features.Commands;

namespace ComponentsApplication.Mappings
{
   public class GeneralMapping : Profile
    {
        public GeneralMapping() {
            CreateMap<CreateComponentCommand, CreateComponentRequestDto>();
            CreateMap<UpdateComponentCommand, UpdateComponentRequestDto>();
        }
    }
}
