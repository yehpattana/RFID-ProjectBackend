
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RFIDApi.Service.Interface;

namespace RFIDApi.Controller.FPS
{
    [ApiController]
    [Route("rfidApi/[controller]")]
    public class PurchasePOController : ControllerBase
    {
        private readonly IPODescService _poDescService;
        private readonly IPODetailService _poDetailService;

        public PurchasePOController(IPODetailService poDetailService, IPODescService pODescService)
        {
            _poDetailService = poDetailService;
            _poDescService = pODescService;
        }

        [Authorize]
        [HttpGet("GetPODesc")]
        public async Task<IActionResult> GetsPODetail()
        {
            var result = await _poDetailService.Gets();
    
            return Ok(result);
        }

        [Authorize]
        [HttpGet("GetPODetail")]
        public async Task<IActionResult> GetsPoDesc()
        {
            var result = await _poDescService.Gets();
            return Ok(result);
        }

        [Authorize]
        [HttpGet("GetPoDesc/options")]
        public async Task<IActionResult> GetPoDescOptions()
        {
            var result = await _poDescService.Options();
            return Ok(result);
        }

        [Authorize]
        [HttpGet("GetPoDetail/{poNo}")]
        public async Task<IActionResult> GetPoDetailById(string poNo)
        {
            var result = await _poDetailService.GetPODetailByPOno(poNo);
            return Ok(result);
        }

    }
}
