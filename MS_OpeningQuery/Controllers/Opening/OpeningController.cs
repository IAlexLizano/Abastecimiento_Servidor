using Microsoft.AspNetCore.Mvc;
using OpeningApplication.Features.Queries;

namespace MS_OpeningQuery.Controllers.Opening
{
    [Route("api/[controller]")]
    [ApiController]
    public class OpeningController : BaseApiController
    {
        [HttpGet("")]
        public async Task<IActionResult> Get([FromQuery] GetBoxByIdQuery request)
        {
            return Ok(await Mediator.Send(request));
        }

        /// <summary>
        /// Obtiene información básica (id y código) de lotes, contenedores, pallets y cajas 
        /// que están en estado "EN_PROCESO"
        /// </summary>
        /// <returns>DTO con listas de información básica</returns>
        [HttpGet("getInformation")]
        public async Task<IActionResult> GetBasicInformation()
        {
            var query = new GetBasicInformationQuery();
            return Ok(await Mediator.Send(query));
        }
    }
}
