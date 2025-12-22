
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

        [HttpGet("Gets/Options/{company}")]

        public async Task<IActionResult> GetOptions(string company)
        {
            var result = await _service.Options(company);
            return Ok(result);
        }


        [HttpGet("AutoRunReceiveNo/{company}")]
        public async Task<IActionResult> GetAutoRunReceiveNo(string company)
        {
            try
            {
                var result = await _service.AutoRunReceiveInNo(company);
                return Ok(result);
            }
            catch (Exception ex) { 
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("CreateReceiveInNo")]
        public async Task<IActionResult> CreateReceiveNo([FromBody]CreateWarehouseReceiveInDTO req)
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

        [HttpPut("UpdateReceiveInNo/{receiveNo}")]
        public async Task<IActionResult> UpdateReceiveInNo(string receiveNo,[FromBody]CreateWarehouseReceiveInDTO req)
        {
            try
            {
                var result = await _service.UpdateWarehouseReceive(receiveNo,req);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeleteReceiveInNo/{receiveNo}")]
        public async Task<IActionResult> DeleteReceiveInNo(string receiveNo)
        {
            try
            {
                var result = await _service.DeleteWarehouseReceive(receiveNo);
                return Ok(result);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetReceiveByNo/{receiveNo}")]
        public async Task<IActionResult> GetReceiveByNo(string receiveNo)
        {
            try
            {
                var res = await _service.GetReceiveIns(receiveNo);
                return Ok(res);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

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
    }
}
