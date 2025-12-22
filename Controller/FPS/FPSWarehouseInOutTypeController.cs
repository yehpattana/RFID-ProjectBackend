
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
    }
}
