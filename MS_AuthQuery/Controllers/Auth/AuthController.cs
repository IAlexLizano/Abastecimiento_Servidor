using AuthApplication.Features.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MS_AuthQuery.Controllers.Auth
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

        [HttpGet("GetMenu")]
        [Authorize]
        public async Task<IActionResult> GetMenu() => Ok(await _mediator.Send(new ObtainMenuQuery()));
    }
}
