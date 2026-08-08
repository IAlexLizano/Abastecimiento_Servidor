using Microsoft.AspNetCore.Mvc;
using OpeningApplication.Features.Queries;

namespace MS_OpeningQuery.Controllers.Boxes
{
    [Route("api/[controller]")]
    [ApiController]
    public class BoxInspectionController : BaseApiController
    {
        [HttpGet("GetBoxById")]
        public async Task<IActionResult> GetBoxById([FromQuery] GetBoxByIdQuery request)
        {
            return Ok(await Mediator.Send(request));
        }

        [HttpGet("GetBoxByUser")]
        public async Task<IActionResult> GetBoxByUser([FromQuery] GetBoxByUserQuery request)
        {
            return Ok(await Mediator.Send(request));
        }

        [HttpGet("GetBoxesByPallet")]
        public async Task<IActionResult> GetBoxesByPallet([FromQuery] GetBoxesByPalletQuery request)
        {
            return Ok(await Mediator.Send(request));
        }

        [HttpGet("GetBoxesByUser")]
        public async Task<IActionResult> GetBoxesByUser([FromQuery] GetAllBoxesByUserQuery request)
        {
            return Ok(await Mediator.Send(request));
        }

        [HttpGet("GetProcessedBoxesToday")]
        public async Task<IActionResult> GetProcessedBoxesToday([FromQuery] GetProcessedBoxesTodayByUserQuery request)
        {
            return Ok(await Mediator.Send(request));
        }
    }
}
