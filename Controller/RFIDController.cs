using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RFIDApi.Models;
using RFIDReaderAPI;
using RFIDReaderAPI.Interface;
using RFIDReaderAPI.Models;
using RFIDApi.Hubs;
using Microsoft.AspNetCore.SignalR;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Concurrent;
using RFIDApi.Models.Context;
using Microsoft.AspNetCore.Authorization;
using Azure;
using RFIDApi.DTO.Data;
namespace RFIDApi.controller
{
    [Route("rfidApi/[controller]")]
    [ApiController]
    public class RFIDController : ControllerBase, RFIDReaderAPI.Interface.IAsynchronousMessage
    {
        string IPConfig = "192.168.1.116:9090";

        private List<string> displayedEpcs = new List<string>();

        private readonly RFIDDbContext _context;
        private readonly FPSDbContext _fbContext;
        private readonly IHubContext<RFIDHubs> _hubContext;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ConcurrentDictionary<string, DateTime> _lastSeen = new();
        private readonly TimeSpan _window = TimeSpan.FromSeconds(1);
        public RFIDController(RFIDDbContext db,FPSDbContext fbContext, IHubContext<RFIDHubs> hubContext, IServiceScopeFactory scopeFactory)
        {
            _context = db;
            _fbContext = fbContext;
            _hubContext = hubContext ?? throw new ArgumentNullException(nameof(hubContext));
            _scopeFactory = scopeFactory;
        }

        [Authorize]
        [HttpPost("StartReading")]
        public async void StartReading([FromBody]string IPconf)
        {
            string ipConf = "";
            if (IPconf.IsNullOrEmpty())
            {
                ipConf = IPConfig;
            }
            else
            {
                ipConf = IPconf;
            }
            bool isConnected = RFIDReader.CreateTcpConn(ipConf, this);
            if (isConnected)
            {
                Debug.WriteLine("Connected to RFID Reader successfully!");
                RFIDReader._Tag6C.GetEPC(IPConfig, eAntennaNo._1 | eAntennaNo._2 | eAntennaNo._3 | eAntennaNo._4, eReadType.Inventory);
                await _hubContext.Clients.All.SendAsync("ReceiveRFIDUpdate", "Connect");
            }
            else
            {
                Debug.WriteLine("Failed to connect to RFID Reader!");
                await _hubContext.Clients.All.SendAsync("ReceiveRFIDUpdate", "NotConnect");
            }
        }

        [HttpPost("StopReading")]
        public void StopReading([FromBody] string IPconf)
        {
            string ipConf = "";
            if (IPconf.IsNullOrEmpty())
            {
                ipConf = IPConfig;
            }
            else
            {
                ipConf = IPconf;
            }
            RFIDReader._RFIDConfig.Stop(ipConf);
            RFIDReader.CloseConn(ipConf);
            Debug.WriteLine($"RFID Has Stop"); // Log ไป Console
        }

        // Return

        [HttpGet("CheckEPC/{epc}")]
        public async Task<IActionResult> CheckEPC(string epc)
        {
            try
            {

                var existTag = await _fbContext.warehouseRFIDs
                    .FirstOrDefaultAsync(t => t.RFID == epc);

                var stockOut = await _fbContext.warehouseTransections.FirstOrDefaultAsync(t => t.RFId == epc && t.OutStatus);
                bool isFound = false;
                bool isOut = false;
                if (existTag != null)
                {
                    isFound = true;
                }
                if (existTag != null && stockOut != null)
                {
                    isOut = true;
                }
                return Ok(new {isOut,isFound, sku = existTag != null ? existTag.SKU : null, });
            }
            catch (Exception ex) { 
                return BadRequest(ex.Message);
            }
        }
        //กดเบิกออก
        [HttpPost("CheckEPCOutStock/{epc}")]
        public async Task<IActionResult> CheckEPCOutStock(string epc, ScanOutStockRequestDto req)
        {
            try
            {

                var existTag = await _fbContext.warehouseRFIDs
                    .FirstOrDefaultAsync(t => t.RFID == epc);

                var inStock = await _fbContext.warehouseRFIDs.FirstOrDefaultAsync(t => t.RFID == epc);
                var canOut = await _fbContext.warehouseTransections.FirstOrDefaultAsync(t => t.RFId == epc && !t.OutStatus);
                bool isFound = false;
                bool isOut = true;
                if(canOut != null)
                {
                    isOut = false;
                }
                if (existTag != null && existTag.ColorCode == req.Color && existTag.Size == req.Size && existTag.ItemCode == req.ProductCode && inStock != null)
                {
                    isFound = true;
                }
                return Ok(new { isOut, isFound, sku = existTag != null ? existTag.SKU : null, });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        void IAsynchronousMessage.EventUpload(CallBackEnum type, object param)
        {
            throw new NotImplementedException();
        }

        void IAsynchronousMessage.GPIControlMsg(GPI_Model gpi_model)
        {
            throw new NotImplementedException();
        }

        //SCAN RFID TAG AND SEND TO CLIENT
        async void IAsynchronousMessage.OutPutTags(Tag_Model tag)
        {
            try
            {
                //if (!ShouldAccept(tag.EPC))
                //{
                //    return;
                //}

                var newTag = new RFIDTag
                {
                    EPC = tag.EPC,
                    RSSI = tag.RSSI_dB,
                    Reader_Name = tag.ReaderName,
                    ANT_NUM = tag.ANT_NUM,
                    ReadTime = DateTime.Now,

                };

                //await Task.Delay(0);
                await _hubContext.Clients.All.SendAsync("ReceiveRFIDData", newTag);
                //RfidSignalRQueue.SignalChannel.Writer.TryWrite(newTag);
                //Debug.WriteLine($"Tag detected and sent to clients: {json}");
                displayedEpcs.Add(tag.EPC);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in OutPutTags: {ex.Message}");
            }
        }

        //[HttpGet("GetSKUFromEPC/{epc}")]
        //public async Task<IActionResult> GetSKUFromEPC(string epc)
        //{
        //    try
        //    {
        //        var res = await _fbContext.warehouseRFIDs.FirstOrDefaultAsync(x => x.RFID == epc);
        //        if(res != null && res.RFID == null)
        //        {
        //            return Ok("");
        //        }
        //        return Ok();
        //    }
        //    catch(Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}
        [NonAction]
        public int SetHF340ANTPower(string connID, Dictionary<int, int> antPower)
        {
            string param = string.Join("|", antPower.Select(kv => $"{kv.Key},{kv.Value}"));
            string response = new RFID_Option().SetReaderPower(connID, param);

            int result = -1;
            if (!string.IsNullOrEmpty(response))
            {
                string[] arr = response.Split('|');
                int.TryParse(arr[0], out result);
            }
            return result;
        }

        [HttpGet("IsReaderConnected")]
        public IActionResult IsReaderConnected()
        {
            // ตรวจสอบว่า reader connect มาแล้วหรือยัง
            bool isConnected = RFIDReader.DIC_CONNECT.ContainsKey(IPConfig);
            return Ok(isConnected);
        }
        void IAsynchronousMessage.OutPutTagsOver()
        {
        }

        void IAsynchronousMessage.PortClosing(string connID)
        {
        }

        void IAsynchronousMessage.PortConnecting(string connID)
        {
        }

        void IAsynchronousMessage.WriteDebugMsg(string msg)
        {
        }

        void IAsynchronousMessage.WriteLog(string msg)
        {
        }

        [NonAction]
        public bool ShouldAccept(string EPC)
        {
            var key = $"{EPC}";
            var now = DateTime.UtcNow;

            if (_lastSeen.TryGetValue(key, out var last))
            {
                if (now - last < _window)
                    return false;
            }

            _lastSeen[key] = now;
            return true;
        }
    }
}
