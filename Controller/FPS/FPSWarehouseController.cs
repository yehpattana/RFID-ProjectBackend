
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RFIDApi.DTO.Data;
using RFIDApi.Service.Interface;

namespace RFIDApi.Controller.FPS
{
    [ApiController]
    [Route("rfidApi/[controller]")]
    public class FPSWarehouseController : ControllerBase
    {
        private readonly IMasterWarehouseService _service;

        public FPSWarehouseController(IMasterWarehouseService service)
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
        [HttpGet("Gets/Options/{company}")]

        public async Task<IActionResult> GetOptions(string company)
        {
            var result = await _service.Options(company);
            return Ok(result);
        }

        [Authorize]
        [HttpGet("AutoRunReceiveNo/{company}")]
        public async Task<IActionResult> GetAutoRunReceiveNo(string company)
        {
            try
            {
                var result = await _service.AutoRunReceiveInNo(company);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPost("CreateReceiveInNo")]
        public async Task<IActionResult> CreateReceiveNo([FromBody] CreateWarehouseReceiveInDTO req)
        {
            try
            {
                var result = await _service.CreateWarehouseReceive(req);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPut("UpdateReceiveInNo/{receiveNo}")]
        public async Task<IActionResult> UpdateReceiveInNo(string receiveNo, [FromBody] CreateWarehouseReceiveInDTO req)
        {
            try
            {
                var result = await _service.UpdateWarehouseReceive(receiveNo, req);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpDelete("DeleteReceiveInNo/{receiveNo}")]
        public async Task<IActionResult> DeleteReceiveInNo(string receiveNo)
        {
            try
            {
                var result = await _service.DeleteWarehouseReceive(receiveNo);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // OutStock

        [Authorize]
        [HttpGet("GetReceiveByNo/{receiveNo}")]
        public async Task<IActionResult> GetReceiveByNo(string receiveNo)
        {
            try
            {
                var res = await _service.GetReceiveIns(receiveNo);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("GetWarehouseRFID")]
        public async Task<IActionResult> GetWarehouseRFID()
        {
            try
            {
                var res = await _service.GetWarehouseRFID();

                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}");
            }
        }

        [Authorize]
        [HttpGet("GetWarehouseRequestOut")]
        public async Task<IActionResult> GetWarehouseRequestOut()
        {
            try
            {
                var res = await _service.GetWarehouseRequestOut();
                if (res.IsSuccess == false)
                {
                    return BadRequest(res);
                }
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}");
            }
        }

        [Authorize]
        [HttpPost("CreateRequestOutstock")]
        public async Task<IActionResult> CreateRequestOutstock([FromBody] WarehouseOutstockDTO req)
        {
            try
            {
                var result = await _service.CreateRequestOutstock(req);
                if (result.IsSuccess == false)
                {
                    return BadRequest(result);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPut("UpdateRequestOutstock/{outNo}")]
        public async Task<IActionResult> CreateRequestOutstock(string outNo, [FromBody] WarehouseOutstockDTO req)
        {
            try
            {
                var result = await _service.UpdateRequestOutstock(outNo, req);
                if (result.IsSuccess == false)
                {
                    return BadRequest(result);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpDelete("DeleteRequestOutstock/{outNo}")]
        public async Task<IActionResult> DeleteRequestOutstock(string outNo)
        {
            try
            {
                var result = await _service.DeleteRequestOutstock(outNo);
                if (result.IsSuccess == false)
                {
                    return BadRequest(result);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("AutoRunOutNo/{company}")]
        public async Task<IActionResult> GetAutoRunOutNo(string company)
        {
            try
            {
                var result = await _service.AutoRunOutNo(company);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPost("CreateWarehouseOutstock")]
        public async Task<IActionResult> CreateWarehouseOutstock(CreateWarehouseOutDTO req)
        {
            var result = await _service.CreateWarehouseOutstock(req);

            if (result.IsSuccess == false)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }


        [Authorize]
        [HttpGet("GetListRequestOutstock")]
        public async Task<IActionResult> GetListRequestOutstock()
        {
            try
            {
                var res = await _service.GetListRequestOutstock();
                if (res.IsSuccess == false)
                {
                    return BadRequest(res);
                }
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}");
            }
        }

        [Authorize]
        [HttpGet("GetRequestOutstockDetail/{outNo}")]
        public async Task<IActionResult> GetRequestOutstockDetail(string outNo)
        {
            try
            {
                var res = await _service.GetWarehouseRequestOutByOutNo(outNo);

                if (res.IsSuccess == false)
                {
                    return BadRequest(res);
                }
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}");
            }
        }

        [Authorize]
        [HttpGet("GetDetailRequest/{requestOutNo}")]
        public async Task<IActionResult> GetDetailRequest(string requestOutNo)
        {
            try
            {
                var res = await _service.GetDetailRequest(requestOutNo);
                if (!res.IsSuccess)
                {
                    return BadRequest(res);
                }
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}");
            }
        }

        [Authorize]
        [HttpPost("GetShowRequestOUT")]
        public async Task<IActionResult> GetShowRequestOUT([FromBody] ShowRequestOutResponseDTO req)
        {
            try
            {
                var res = await _service.GetShowRequestOUT(req);
                if (!res.IsSuccess)
                {
                    return BadRequest(res);
                }
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}");
            }
        }

        [Authorize]
        [HttpGet("GetRequestMainByOutNo/{outNo}")]

        public async Task<IActionResult> GetRequestMainByOutNo(string outNo)
        {
            try
            {
                var res = await _service.GetRequestMainByOutNo(outNo);
                if (!res.IsSuccess)
                {
                    return BadRequest(res);
                }
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}");
            }
        }
    }
}
