using SupplyingApplication.Features.Queries;
using Microsoft.AspNetCore.Mvc;

namespace MS_SupplyingQuery.Controllers.Supplying
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplyController : BaseApiController
    {
        /// <summary>
        /// Obtiene cartones pendientes de abastecimiento
        /// Solo devuelve aquellos con estado PENDIENTE
        /// </summary>
        /// <returns>Lista de cartones con detalles completos</returns>
        [HttpGet("GetPendingBoxes")]
        public async Task<IActionResult> GetPendingBoxes()
        {
            return Ok(await Mediator.Send(new GetPendingBoxesQuery()));
        }

        /// <summary>
        /// Obtiene supplies en estado EN_PROCESO
        /// </summary>
        /// <returns>Lista de cajas en proceso con detalles</returns>
        [HttpGet("GetSuppliesInProgress")]
        public async Task<IActionResult> GetSuppliesInProgress()
        {
            return Ok(await Mediator.Send(new GetSuppliesInProgressQuery()));
        }

        /// <summary>
        /// Obtiene todos los cartones sin importar el estado
        /// </summary>
        /// <returns>Lista completa de cartones con todos los detalles</returns>
        [HttpGet("GetAllBoxes")]
        public async Task<IActionResult> GetAllBoxes()
        {
            return Ok(await Mediator.Send(new GetAllBoxesQuery()));
        }
    }
}
