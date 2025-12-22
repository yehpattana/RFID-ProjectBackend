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
        private readonly TimeSpan _window = TimeSpan.FromSeconds(3);
        public RFIDController(RFIDDbContext db,FPSDbContext fbContext, IHubContext<RFIDHubs> hubContext, IServiceScopeFactory scopeFactory)
        {
            _context = db;
            _fbContext = fbContext;
            _hubContext = hubContext ?? throw new ArgumentNullException(nameof(hubContext));
            _scopeFactory = scopeFactory;
        }

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


        void IAsynchronousMessage.EventUpload(CallBackEnum type, object param)
        {
            throw new NotImplementedException();
        }

        void IAsynchronousMessage.GPIControlMsg(GPI_Model gpi_model)
        {
            throw new NotImplementedException();
        }

        async void IAsynchronousMessage.OutPutTags(Tag_Model tag)
        {
            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    if (!ShouldAccept(tag.EPC))
                    {
                        return;
                    }
                    var context = scope.ServiceProvider.GetRequiredService<RFIDDbContext>();
                    var fpsContext = scope.ServiceProvider.GetRequiredService<FPSDbContext>();
                    var existTag = await context.ProductsRFID
                        .FirstOrDefaultAsync(t => t.RFID == tag.EPC);

                    var stockOut = await fpsContext.warehouseTransections.FirstOrDefaultAsync(t => t.RFId == tag.EPC && t.OutStatus);
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
                    var newTag = new RFIDTag
                    {
                        EPC = tag.EPC,
                        RSSI = tag.RSSI_dB,
                        Reader_Name = tag.ReaderName,
                        ANT_NUM = tag.ANT_NUM,
                        ReadTime = DateTime.Now,
                        sku = existTag != null ? existTag.SKU : null,
                        isFound = isFound,
                        isOut = isOut
                    };

                    await _hubContext.Clients.All.SendAsync("ReceiveRFIDData", newTag);

                    //Debug.WriteLine($"Tag detected and sent to clients: {json}");
                    displayedEpcs.Add(tag.EPC);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in OutPutTags: {ex.Message}");
            }
        }
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
