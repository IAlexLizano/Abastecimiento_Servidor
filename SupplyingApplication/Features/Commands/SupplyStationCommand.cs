using MediatR;
using Shared.Application.Wrappers;
using SupplyingApplication.DTOs;
using SupplyingApplication.Interfaces;

namespace SupplyingApplication.Features.Commands
{
    /// <summary>
    /// Command para abastecer una estación
    /// Registra el envío de cartones y actualiza estados en cascada
    /// </summary>
    public class SupplyStationCommand : IRequest<Response<SupplyDto>>
    {
        public int IdSupply { get; set; }
        public int IdUserSend { get; set; }
    }

    /// <summary>
    /// Handler del comando de abastecimiento de estación
    /// </summary>
    public class SupplyStationCommandHandler : IRequestHandler<SupplyStationCommand, Response<SupplyDto>>
    {
        private readonly ISupplyingRepository _repository;

        public SupplyStationCommandHandler(ISupplyingRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<Response<SupplyDto>> Handle(SupplyStationCommand request, CancellationToken cancellationToken)
        {
            var result = await _repository.SupplyStationAsync(request.IdSupply, request.IdUserSend);
            return new Response<SupplyDto>(result, "Estación abastecida exitosamente");
        }
    }
}
