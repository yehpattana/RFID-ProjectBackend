
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RFIDApi.DTO.Data;
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

        [Authorize]
        [HttpGet("Options")]
        public async Task<IActionResult> GetsWCompany()
        {
            var result = await _service.Options();
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
        [HttpGet("CheckDetail")]
        public async Task<IActionResult> GetCheckDetail([FromQuery]CheckRequestOutstock req)
        {
            var result = await _service.GetCheckDetail(req.ItemCode,req.ColorCode,req.Size);
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
        [HttpGet("GetUOM")]
        public async Task<IActionResult> GetUOM([FromQuery]CheckRequestOutstock req)
        {
            var result = await _service.GetUOM(req.ItemCode, req.ColorCode, req.Size);
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
