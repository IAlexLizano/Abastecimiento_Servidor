using AuthApplication.Features.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MS_AuthQuery.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : BaseApiController
    {
        [HttpGet("GetMenu")]
        [Authorize]
        public async Task<IActionResult> GetMenu() => Ok(await Mediator.Send(new ObtainMenuQuery()));
    }
}
