using AutoMapper;
using PreOpeningApplication.DTOs;
using PreOpeningApplication.Features.Commands;

namespace PreOpeningApplication.Mappings
{
   public class GeneralMapping : Profile
    {
        public GeneralMapping() {
            CreateMap<CreateLabeledBoxesAsyncCommand, CreateLabeledBoxesRequestDto>();
            CreateMap<LoadCkdLotAsyncCommand, LoadCkdLotRequestDto>();
            CreateMap<StoreContainerAsyncCommand, StoreContainerRequestDto>();
            CreateMap<StorePalletAsyncCommand, StorePalletRequestDto>();
        }
    }
}
