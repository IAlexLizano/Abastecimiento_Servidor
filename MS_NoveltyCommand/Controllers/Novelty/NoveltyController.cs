using NoveltyApplication.Features.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MS_NoveltyCommand.Controllers.Novelty
{
    [Route("api/[controller]")]
    [ApiController]
    public class NoveltyController : BaseApiController
    {
        [HttpPost("RegisterNovelty")]
        public async Task<IActionResult> RegisterNovelty(RegisterNoveltyCommand request)
        {
            return Ok(await Mediator.Send(request));
        }

        [HttpPut("ResolveNovelty")]
        public async Task<IActionResult> ResolveNovelty(ResolveNoveltyCommand request)
        {
            return Ok(await Mediator.Send(request));
        }

        [HttpPut("AcceptNovelty")]
        public async Task<IActionResult> AcceptNovelty(AcceptNoveltyCommand request)
        {
            return Ok(await Mediator.Send(request));
        }
    }
}
