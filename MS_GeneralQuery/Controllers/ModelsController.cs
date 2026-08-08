using GeneralApplication.Features.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MS_GeneralQuery.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ModelsController : BaseApiController
    {
        [HttpGet]
        [Route("GetAllModels")]
        public async Task<IActionResult> GetAllModels()
        {
            return Ok(await Mediator.Send(new GetAllModelsQuery()));
        }
    }
}
