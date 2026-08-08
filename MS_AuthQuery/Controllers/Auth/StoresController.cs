using AuthApplication.Features.Queries.Stores;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MS_AuthQuery.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StoresController : BaseApiController
    {
        /// <summary>
        /// Obtiene todas las tiendas disponibles
        /// </summary>
        /// <returns>Lista de todas las tiendas</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllStores()
        {
            var query = new GetAllStoresQuery();
            return Ok(await Mediator.Send(query));
        }
    }
}
