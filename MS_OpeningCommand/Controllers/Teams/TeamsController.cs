using OpeningApplication.Features.Commands;
using Microsoft.AspNetCore.Mvc;

namespace MS_OpeningCommand.Controllers.Teams
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsController : BaseApiController
    {
        [HttpPost("AssignTeamToBox")]
        public async Task<IActionResult> AssignTeamToBox(AssignTeamToBoxCommand request)
        {
            return Ok(await Mediator.Send(request));
        }
    }
}
