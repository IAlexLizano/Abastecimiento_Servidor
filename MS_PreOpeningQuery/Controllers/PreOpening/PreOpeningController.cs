using PreOpeningApplication.Features.Queries;
using Microsoft.AspNetCore.Mvc;

namespace MS_PreOpeningQuery.Controllers.PreOpening
{
    [Route("api/[controller]")]
    [ApiController]
    public class PreOpeningController : BaseApiController
    {
        [HttpGet("GenerateContainerUnloadFormat")]
        public async Task<IActionResult> GenerateContainerUnloadFormat([FromQuery] GenerateContainerUnloadFormatAsyncQuery request)
        {
            return Ok(await Mediator.Send(request));
        }

        [HttpGet("GenerateUnpackingFormat")]
        public async Task<IActionResult> GenerateUnpackingFormat([FromQuery] GenerateUnpackingFormatAsyncQuery request)
        {
            return Ok(await Mediator.Send(request));
        }

        [HttpGet("GetCkdLotWithDetails")]
        public async Task<IActionResult> GetCkdLotWithDetails([FromQuery] GetCkdLotWithDetailsAsyncQuery request)
        {
            return Ok(await Mediator.Send(request));
        }
        
        [HttpGet("GetContainersWithPalletsByCkd")]
        public async Task<IActionResult> GetContainersWithPalletsByCkd([FromQuery] GetContainersWithPalletsByCkdAsyncQuery request)
        {
            return Ok(await Mediator.Send(request));
        }

        [HttpGet("GetPalletsByContainer")]
        public async Task<IActionResult> GetPalletsByContainer([FromQuery] GetPalletsByContainerAsyncQuery request)
        {
            return Ok(await Mediator.Send(request));
        }

        [HttpGet("GetAllCkdLots")]
        public async Task<IActionResult> GetAllCkdLots([FromQuery] GetAllCkdLotsAsyncQuery request)
        {
            return Ok(await Mediator.Send(request));
        }

        [HttpGet("GetAllContainersByCkdLot")]
        public async Task<IActionResult> GetAllContainersByCkdLot([FromQuery] GetAllContainersByCkdLotAsyncQuery request)
        {
            return Ok(await Mediator.Send(request));
        }

        [HttpGet("GetAllPalletsByContainer")]
        public async Task<IActionResult> GetAllPalletsByContainer([FromQuery] GetAllPalletsByContainerAsyncQuery request)
        {
            return Ok(await Mediator.Send(request));
        }
    }
}
