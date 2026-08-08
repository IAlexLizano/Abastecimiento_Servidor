using PreOpeningApplication.Features.Commands;
using Microsoft.AspNetCore.Mvc;

namespace MS_PreOpeningCommand.Controllers.PreOpening
{
    [Route("api/[controller]")]
    [ApiController]
    public class PreOpeningController : BaseApiController
    {
        [HttpPost("CreateLabeledBoxes")]
        public async Task<IActionResult> CreateLabeledBoxes(CreateLabeledBoxesAsyncCommand request)
        {
            return Ok(await Mediator.Send(request));
        }

        [HttpPost("LoadCkdLot")]
        public async Task<IActionResult> LoadCkdLot(LoadCkdLotAsyncCommand request)
        {
            return Ok(await Mediator.Send(request));
        }

        [HttpPost("StoreContainer")]
        public async Task<IActionResult> StoreContainer(StoreContainerAsyncCommand request)
        {
            return Ok(await Mediator.Send(request));
        }

        [HttpPost("StorePallet")]
        public async Task<IActionResult> StorePallet(StorePalletAsyncCommand request)
        {
            return Ok(await Mediator.Send(request));
        }
    }
}
