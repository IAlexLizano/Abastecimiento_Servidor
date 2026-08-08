using GeneralApplication.DTOs.Dashboard;
using GeneralApplication.Features.Dashboard.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MS_GeneralQuery.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DashboardController : BaseApiController
    {
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] DashboardFilterDto filter)
        {
            var result = await Mediator.Send(new GetDashboardKpisQuery { Filter = filter });
            return Ok(result);
        }
    }
}
