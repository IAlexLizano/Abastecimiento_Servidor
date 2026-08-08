using AuthApplication.Features.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MS_AuthCommand.Controllers.Auth
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : BaseApiController
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginCommand request)
        {
            return Ok(await _mediator.Send(request));
        }
    } 
}