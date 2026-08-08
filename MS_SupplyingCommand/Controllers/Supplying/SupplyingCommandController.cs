using SupplyingApplication.Features.Commands;
using Microsoft.AspNetCore.Mvc;

namespace MS_SupplyingCommand.Controllers.Supplying
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplyController : BaseApiController
    {
        /// <summary>
        /// Abastece una estación registrando el envío de cartones
        /// </summary>
        /// <param name="request">Datos del supply a abastecer</param>
        /// <returns>DTO del supply actualizado</returns>
        [HttpPost("SupplyStation")]
        public async Task<IActionResult> SupplyStation(SupplyStationCommand request)
        {
            return Ok(await Mediator.Send(request));
        }

        /// <summary>
        /// Recibe un cartón registrando la recepción
        /// </summary>
        /// <param name="request">Datos del supply a recibir</param>
        /// <returns>DTO del supply actualizado</returns>
        [HttpPost("ReceiveBox")]
        public async Task<IActionResult> ReceiveBox(ReceiveBoxCommand request)
        {
            return Ok(await Mediator.Send(request));
        }
    }
}
