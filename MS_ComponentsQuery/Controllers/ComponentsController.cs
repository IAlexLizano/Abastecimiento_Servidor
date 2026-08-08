using ComponentsApplication.Features.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MS_AuthQuery.Controllers;

namespace MS_ComponentsQuery.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ComponentsController : BaseApiController
    {
        [HttpGet]
        [Route("GetAllComponents")]
        public async Task<IActionResult> GetAllComponents()
        {
            return Ok(await Mediator.Send(new GetAllComponentsQuery()));
        }
        [HttpGet]
        [Route("GetComponentById")]
        public async Task<IActionResult> GetComponentById([FromQuery] GetComponentByIdQuery request)
        {
            return Ok(await Mediator.Send(request));

        }

        [HttpGet]
        [Route("GetRecipesByModel")]
        public async Task<IActionResult> GetRecipesByModel([FromQuery] GetRecipesByModelQuery request)
        {
            return Ok(await Mediator.Send(request));
        }
    }
}
