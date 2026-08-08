using Microsoft.AspNetCore.Mvc;
using OpeningApplication.Features.Queries;

namespace MS_OpeningQuery.Controllers.Teams
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsController : BaseApiController
    {
        [HttpGet("GetTeamAssignmentByBox")]
        public async Task<IActionResult> GetTeamAssignmentByBox([FromQuery] GetTeamAssignmentByBoxQuery request)
        {
            return Ok(await Mediator.Send(request));
        }
    }
}