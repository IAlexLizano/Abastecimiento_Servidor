using OpeningApplication.Features.Commands;
using Microsoft.AspNetCore.Mvc;

namespace MS_OpeningCommand.Controllers.Boxes
{
    [Route("api/[controller]")]
    [ApiController]
    public class BoxInspectionController : BaseApiController
    {
        [HttpPost("RegisterBoxOpening")]
        public async Task<IActionResult> RegisterBoxOpening(RegisterBoxOpeningCommand request)
        {
            return Ok(await Mediator.Send(request));
        }

        [HttpPut("RegisterUnboxing")]
        public async Task<IActionResult> RegisterUnboxing(RegisterUnboxingCommand request)
        {
            return Ok(await Mediator.Send(request));
        }
    }
}
