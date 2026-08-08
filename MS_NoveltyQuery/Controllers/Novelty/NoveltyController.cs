using NoveltyApplication.Features.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MS_NoveltyQuery.Controllers.Novelty
{
    [Route("api/[controller]")]
    [ApiController]
    public class NoveltyController : BaseApiController
    {
        [HttpGet("GetNoveltyByBox/{boxId}")]
        public async Task<IActionResult> GetNoveltyByBox(int boxId)
        {
            var query = new GetNoveltyByBoxQuery { BoxId = boxId };
            return Ok(await Mediator.Send(query));
        }

        [HttpGet("GetNoveltyByPallet/{palletId}")]
        public async Task<IActionResult> GetNoveltyByPallet(int palletId)
        {
            var query = new GetNoveltyByPalletQuery { PalletId = palletId };
            return Ok(await Mediator.Send(query));
        }


        [HttpGet("GetNoveltyById/{issueId}")]
        public async Task<IActionResult> GetNoveltyById(int issueId)
        {
            var query = new GetNoveltyByIdQuery { IssueId = issueId };
            return Ok(await Mediator.Send(query));
        }

        [HttpGet("GetOpenNovelties")]
        public async Task<IActionResult> GetOpenNovelties()
        {
            var query = new GetOpenNoveltiesQuery();
            return Ok(await Mediator.Send(query));
        }

        [HttpGet("GetNoveltyByUser/{userId}")]
        public async Task<IActionResult> GetNoveltyByUser(int userId)
        {
            var query = new GetNoveltyByUserQuery { UserId = userId };
            return Ok(await Mediator.Send(query));
        }
    }
}
