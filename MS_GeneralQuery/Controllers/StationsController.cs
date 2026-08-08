using GeneralApplication.Features.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MS_GeneralQuery.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StationsController : BaseApiController
    {
        [HttpGet]
        [Route("GetAllStations")]
        public async Task<IActionResult> GetAllStations()
        {
            return Ok(await Mediator.Send(new GetAllStationsQuery()));
        }
    }
}
