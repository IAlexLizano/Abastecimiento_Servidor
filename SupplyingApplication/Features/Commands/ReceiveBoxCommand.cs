using MediatR;
using Shared.Application.Wrappers;
using SupplyingApplication.DTOs;
using SupplyingApplication.Interfaces;

namespace SupplyingApplication.Features.Commands
{
    /// <summary>
    /// Command para recibir un cartón
    /// Registra la recepción y actualiza estados en cascada
    /// </summary>
    public class ReceiveBoxCommand : IRequest<Response<SupplyDto>>
    {
        public int IdSupply { get; set; }
        public int IdUserReceive { get; set; }
    }

    /// <summary>
    /// Handler del comando de recepción de cartón
    /// </summary>
    public class ReceiveBoxCommandHandler : IRequestHandler<ReceiveBoxCommand, Response<SupplyDto>>
    {
        private readonly ISupplyingRepository _repository;

        public ReceiveBoxCommandHandler(ISupplyingRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<Response<SupplyDto>> Handle(ReceiveBoxCommand request, CancellationToken cancellationToken)
        {
            var result = await _repository.ReceiveBoxAsync(request.IdSupply, request.IdUserReceive);
            return new Response<SupplyDto>(result, "Cartón recibido exitosamente");
        }
    }
}
