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
namespace RFIDApi.controller
{
    [Route("rfidApi/[controller]")]
    [ApiController]
    public class RFIDController : ControllerBase, RFIDReaderAPI.Interface.IAsynchronousMessage
    {
        string IPConfig = "192.168.1.116:9090";

        private List<string> displayedEpcs = new List<string>();

        private readonly RFIDDbContext _context;
        private readonly IHubContext<RFIDHubs> _hubContext;
        private readonly IServiceScopeFactory _scopeFactory;
        public RFIDController(RFIDDbContext db, IHubContext<RFIDHubs> hubContext, IServiceScopeFactory scopeFactory)
        {
            _context = db;
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
                    var context = scope.ServiceProvider.GetRequiredService<RFIDDbContext>();

                    var newTag = new RFIDTag
                    {
                        EPC = tag.EPC,
                        RSSI = tag.RSSI_dB,
                        Reader_Name = tag.ReaderName,
                        ANT_NUM = tag.ANT_NUM,
                        ReadTime = DateTime.Now,

                    };

                    await _hubContext.Clients.All.SendAsync("ReceiveRFIDData", tag.EPC);

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
    }
}
