using ComponentsApplication.Features.Commands;
using ComponentsApplication.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MS_AuthQuery.Controllers;
using NoveltyApplication.Features.Commands;

namespace MS_ComponentsCommand.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ComponentsController : BaseApiController
    {
        [HttpPost]
        [Route("CreateComponent")]
        public async Task<IActionResult> CreateComponent(CreateComponentCommand request)
        {
            return Ok(await Mediator.Send(request));

        }
        [HttpPut]
        [Route("UpdateComponent")]
        public async Task<IActionResult> UpdateComponent(UpdateComponentCommand request)
        {
            return Ok(await Mediator.Send(request));

        }
        [HttpPost]
        [Route("LoadRecipe")]
        public async Task<IActionResult> LoadRecipe(LoadRecipeCommand request)
        {
            return Ok(await Mediator.Send(request));
        }
    }
}
