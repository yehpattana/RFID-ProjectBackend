
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RFIDApi.Service.Interface;

namespace RFIDApi.Controller.FPS
{
    [ApiController]
    [Route("rfidApi/[controller]")]
    public class FPSWarehouseInOutTypeController : ControllerBase
    {
        private readonly IWarehouseInOutTypeService _service;

        public FPSWarehouseInOutTypeController(IWarehouseInOutTypeService service)
        {
            _service = service;
        }

        [Authorize]
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

        [Authorize]
        [HttpGet("OutOptions")]
        public async Task<IActionResult> OutOptions()
        {
            var result = await _service.OutOptions();
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
