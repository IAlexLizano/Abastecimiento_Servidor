using AuthApplication.Features.Commands;
using Microsoft.AspNetCore.Mvc;

namespace MS_AuthCommand.Controllers.Auth
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : BaseApiController
    {
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginCommand request)
        {
            return Ok(await Mediator.Send(request));
        }
    } 
}