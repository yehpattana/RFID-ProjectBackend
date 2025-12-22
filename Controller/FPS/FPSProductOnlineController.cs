
using Microsoft.AspNetCore.Mvc;
using RFIDApi.Service.Interface;

namespace RFIDApi.Controller.FPS
{
    [ApiController]
    [Route("rfidApi/[controller]")]
    public class FPSProductOnlineController : ControllerBase
    {
        private readonly IMasterProductOnlineService _service;

        public FPSProductOnlineController(IMasterProductOnlineService service)
        {
            _service = service;
        }

        [HttpGet("Gets")]
        public async Task<IActionResult> Gets()
        {
            var result = await _service.Gets();
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpGet("GetWithDetail")]
        public async Task<IActionResult> GetWithDetail()
        {
            var result = await _service.GetWithDetail();
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
    }
}
