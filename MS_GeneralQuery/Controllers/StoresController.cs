using GeneralApplication.Features.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MS_GeneralQuery.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StoresController : BaseApiController
    {
        [HttpGet]
        [Route("GetAllStores")]
        public async Task<IActionResult> GetAllStores()
        {
            return Ok(await Mediator.Send(new GetAllStoresQuery()));
        }
    }
}
