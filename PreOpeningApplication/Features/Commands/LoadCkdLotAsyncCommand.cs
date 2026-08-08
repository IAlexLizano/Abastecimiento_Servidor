using AutoMapper;
using MediatR;
using PreOpeningApplication.DTOs;
using PreOpeningApplication.Interfaces;
using Shared.Application.Wrappers;

namespace PreOpeningApplication.Features.Commands
{
    public class LoadCkdLotAsyncCommand : IRequest<Response<string>>
    {
        public string LotCode { get; set; } = null!;
        public string Product { get; set; } = null!;
        public DateTime? ArrivalDate { get; set; }
        public List<LoadContainerRequestDto> Containers { get; set; } = new();
    }

    public class LoadCkdLotAsyncCommandHandler : IRequestHandler<LoadCkdLotAsyncCommand, Response<string>>
    {
        private readonly IPreOpeningRepository _repository;
        private readonly IMapper _mapper;
        public LoadCkdLotAsyncCommandHandler(IPreOpeningRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response<string>> Handle(LoadCkdLotAsyncCommand request, CancellationToken cancellationToken)
        {
                var requestDto = _mapper.Map<LoadCkdLotAsyncCommand, LoadCkdLotRequestDto>(request);
                var result = await _repository.LoadLotAsync(requestDto);
                return new Response<string>(result, "Lote cargado exitosamente");
        }
    }
}
