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

        //public void StartReading()
        //{
        //    bool isConnected = RFIDReader.CreateTcpConn(IPConfig, this);
        //    if (isConnected)
        //    {
        //        Debug.WriteLine("Connected to RFID Reader successfully!");
        //        RFIDReader._Tag6C.GetEPC(IPConfig, eAntennaNo._1 | eAntennaNo._2 | eAntennaNo._3 | eAntennaNo._4, eReadType.Inventory);
        //    }
        //    else
        //    {
        //        Debug.WriteLine("Failed to connect to RFID Reader!");
        //    }
        //}


        //[HttpGet("GetReaderPower")]
        //public IActionResult SetReader()
        //{
        //    var ant1 = new RFIDConfig();

        //    var antPower = new Dictionary<int, int>
        //    {
        //        { 1, 15 },
        //        { 2, 30 },
        //        { 3, 30 },
        //        { 4, 30 },
        //    };

        //    var result = SetHF340ANTPower(IPConfig, antPower);
        //    Console.WriteLine($"Set Result: {result}");

        //    var getResult = ant1.GetANTPowerParam(IPConfig);
        //    Console.WriteLine($"Get Result: {string.Join(", ", getResult.Select(kv => kv.Key + "=" + kv.Value))}");

        //    return Ok(getResult);
        //}

        //[HttpPost("OpenTCPServer")]
        //public async Task<IActionResult> OpenServer()
        //{
        //    try
        //    {
        //        bool isOpened = RFIDReader.OpenTcpServer("0.0.0.0", "6000", this);

        //        if (!isOpened)
        //        {
        //            return NotFound("Cant open Server");
        //        }

        //        return Ok("Open Success");
        //    }
        //    catch (Exception ex) { 
        //        return BadRequest(ex);
        //    }
        //}

        //[HttpGet("CheckConnectionRFID")]
        //public async Task<IActionResult> CheckConnectionDevice()
        //{
        //    try
        //    {
        //        bool isConnected = RFIDReader.CheckConnect(IPConfig);
        //        return Ok(new { isConnect = isConnected });
        //    }
        //    catch (Exception ex) { 
        //        return BadRequest(ex);
        //    }
        //}
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
                //RFIDReader._Tag6C.GetEPC(IPConfig, eAntennaNo._1 , eReadType.Inventory);
                await _hubContext.Clients.All.SendAsync("ReceiveRFIDUpdate", "Connect");
            }
            else
            {
                Debug.WriteLine("Failed to connect to RFID Reader!");
                await _hubContext.Clients.All.SendAsync("ReceiveRFIDUpdate", "NotConnect");
            }
        }

        //[HttpPost("ReStartReading")]
        //public void ReStartReading()
        //{
        //    //var delRFID = _context.RFIDTags.ToList(); // ดึงข้อมูลทั้งหมดจากตาราง RFIDTags
        //    //_context.RFIDTags.RemoveRange(delRFID);   // ลบข้อมูลทั้งหมด
        //    //_context.SaveChanges();
        //    Debug.WriteLine($"RFID Has Restart"); // Log ไป Console
        //    if (RFIDReader.CreateTcpConn(IPConfig, this))
        //    {
        //        RFIDReader._Tag6C.GetEPC(IPConfig, eAntennaNo._1 | eAntennaNo._2 | eAntennaNo._3 | eAntennaNo._4, eReadType.Inventory);
        //    }
        //}

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
            //var delRFID = _context.RFIDTags.ToList(); // ดึงข้อมูลทั้งหมดจากตาราง RFIDTags
            //_context.RFIDTags.RemoveRange(delRFID);   // ลบข้อมูลทั้งหมด
            //_context.SaveChanges();
        }

        //[HttpGet("GetTags")]
        //public ActionResult GetTags()
        //{
        //    // ประกาศ List เพื่อเก็บผลลัพธ์ทั้งหมด
        //    List<Product> allData = new List<Product>();

        //    // วนลูปผ่าน TagsRFID
        //    var TagsRFID = _context.POS_NDS_RFIDs.ToList();
        //    foreach (var d in TagsRFID)
        //    {
        //        // ดึงข้อมูลจาก Products ตามเงื่อนไข และเพิ่มลงใน allData
        //        var data = _context.Products.Where(z => z.RFIDData == d.RFIDCode).ToList();
        //        allData.AddRange(data);
        //    }

        //    return new JsonResult(allData);

        //}

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
                        ReadTime = DateTime.Now,
                        IsActive = 1
                    };

                    //var Product = context.POS_NDS_Variants
                    //    .Include(z => z.Product)
                    //        .ThenInclude(p => p.Category)
                    //    .Include(z => z.Size)
                    //    .Include(z => z.ColorCode)
                    //    .Include(z => z.StockInShops)
                    //        .ThenInclude(s => s.Shop)
                    //    .Include(z => z.RFIDs)
                    //    .Where(z => z.RFIDs != null && z.RFIDs.Any(r => r.RFIDCode == tag.EPC))
                    //    .Select(z => new
                    //    {
                    //        productId = z.ProductId,
                    //        variantId = z.VariantId,
                    //        productName = z.Product != null ? z.Product.ProductName : "N/A",
                    //        productDescription = z.Product != null ? z.Product.ProductDescription : "N/A",
                    //        colorId = z.ColorId,
                    //        color = z.ColorCode != null ? z.ColorCode.ColorName : "N/A",
                    //        sizeId = z.SizeId,
                    //        size = z.Size != null ? z.Size.SizeName : "N/A",
                    //        price = z.Price,
                    //        categoryId = z.Product != null && z.Product.Category.CategoryId != null ? z.Product.CategoryId : (int?)null,
                    //        rfidData = z.RFIDs.FirstOrDefault(w => w.RFIDCode == tag.EPC).RFIDCode,
                    //        quantityInStock = z.StockInShops.FirstOrDefault(s => s.Shop != null && s.Shop.ShopId == 1).Qty,
                    //        status = z.Status
                    //    })
                    //    .ToList();
                    //var json = JsonSerializer.Serialize(Product, new JsonSerializerOptions
                    //{
                    //    WriteIndented = true,
                    //    ReferenceHandler = ReferenceHandler.Preserve
                    //});

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
