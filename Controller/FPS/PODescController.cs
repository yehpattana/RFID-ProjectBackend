using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RFIDApi.Service.Interface;

namespace RFIDApi.Controller.FPS
{
    [ApiController]
    [Route("rfidApi/[controller]")]
    public class PODescController : ControllerBase
    {
        private readonly IPODescService _service;
        public PODescController(IPODescService service)
        {
            _service = service;
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
