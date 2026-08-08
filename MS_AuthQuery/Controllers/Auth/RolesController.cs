using AuthApplication.Features.Queries.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MS_AuthQuery.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RolesController : BaseApiController
    {
        /// <summary>
        /// Obtiene todos los roles disponibles
        /// </summary>
        /// <returns>Lista de todos los roles</returns>
        [HttpGet("GetRoles")]
        public async Task<IActionResult> GetAllRoles()
        {
            var query = new GetAllRolesQuery();
            return Ok(await Mediator.Send(query));
        }
    }
}
