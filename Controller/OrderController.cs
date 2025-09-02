//using System.Diagnostics;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using RFIDApi.Models;
//using RFIDReaderAPI;
//using RFIDReaderAPI.Interface;
//using RFIDReaderAPI.Models;
//using RFIDApi.Hubs;
//using Microsoft.AspNetCore.SignalR;
//using System.Text.Json;
//using System.Text.Json.Serialization;
//using System.Security.Claims;
//using System.Transactions;
//namespace RFIDApi.controller
//{
//    [Route("rfidApi/[controller]")]
//    [ApiController]
//    public class OrderController : ControllerBase
//    {
//        string IPConfig = "192.168.1.116:9090";

//        private List<string> displayedEpcs = new List<string>();

//        private readonly RFIDDbContext _context;
//        private readonly IHubContext<RFIDHubs> _hubContext;
//        private readonly IServiceScopeFactory _scopeFactory;
//        public OrderController(RFIDDbContext db, IHubContext<RFIDHubs> hubContext, IServiceScopeFactory scopeFactory)
//        {
//            _context = db;
//            _hubContext = hubContext ?? throw new ArgumentNullException(nameof(hubContext));
//            _scopeFactory = scopeFactory;
//        }

//        //public void StartReading()
//        //{
//        //    bool isConnected = RFIDReader.CreateTcpConn(IPConfig, this);
//        //    if (isConnected)
//        //    {
//        //        Debug.WriteLine("Connected to RFID Reader successfully!");
//        //        RFIDReader._Tag6C.GetEPC(IPConfig, eAntennaNo._1 | eAntennaNo._2 | eAntennaNo._3 | eAntennaNo._4, eReadType.Inventory);
//        //    }
//        //    else
//        //    {
//        //        Debug.WriteLine("Failed to connect to RFID Reader!");
//        //    }
//        //}
//        [HttpPost("CreateOrderTransaction")]
//        public ActionResult CreateOrderTransaction(CreateOrderTransactionRequestDTO[] datas)
//        {
//            var errors = new List<string>();

//            try
//            {
//                var identity = (ClaimsIdentity)User.Identity;
//                var shopIdClaim = identity.FindFirst("ShopId");


//                if (shopIdClaim == null || !int.TryParse(shopIdClaim.Value, out int shopId))
//                {
//                    return new JsonResult(new { success = false, data = new List<object>(), message = "ShopId is invalid or not found in token" }, JsonRequestBehavior.AllowGet);
//                }
//                // ตรวจสอบข้อมูลที่ส่งมา
//                if (datas == null || !datas.Any())
//                {
//                    errors.Add("No data provided.");
//                    return new JsonResult(new { error = errors });
//                }

//                foreach (var data in datas)
//                {
//                    if (data == null)
//                        errors.Add("Data is null.");

//                    if (data.ProductId <= 0)
//                        errors.Add($"Invalid ProductId: {data.ProductId}");

//                    if (string.IsNullOrEmpty(data.ColorName))
//                        errors.Add($"ColorName is required for ProductId: {data.ProductId}");

//                    if (string.IsNullOrEmpty(data.SizeName))
//                        errors.Add($"SizeName is required for ProductId: {data.ProductId}");

//                    if (data.Quantity <= 0)
//                        errors.Add($"Invalid Quantity: {data.Quantity} for ProductId: {data.ProductId}");

//                    if (string.IsNullOrEmpty(data.CreatedBy))
//                        errors.Add($"CreatedBy is required for ProductId: {data.ProductId}");

//                    if (data.PaymentMethod == null || data.PaymentMethod == "")
//                        errors.Add($"PaymentMethod is Require : {data.PaymentMethod}");
//                }

//                if (errors.Any())
//                    return new JsonResult(new { error = errors });

//                // ใช้ TransactionScope เพื่อครอบการทำงานทั้งหมด
//                using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
//                {
//                    // สร้าง Order
//                    var paymentMethodName = datas[0]?.PaymentMethod?.Trim();
//                    var existPaymentMethod = _context.POS_NDS_PaymentMethods
//                                            .FirstOrDefault(e => e.PMNAME.Equals(paymentMethodName, StringComparison.OrdinalIgnoreCase));
//                    if (existPaymentMethod == null)
//                        return new JsonResult(new { error = $"Payment method '{paymentMethodName}' not found." });
//                    var order = new POS_NDS_Order
//                    {
//                        OrderCode = GenerateOrderId(shopId), // ควรใช้การสร้าง OrderId ที่ไม่ซ้ำกัน เช่น ใช้ Guid หรือ Auto-increment
//                        TotalAmount = datas[0].TotalAmount, // คำนวณ TotalAmount จากข้อมูลทั้งหมด
//                        ReceivedAmount = datas[0].ReceivePaid,
//                        ChangeAmount = datas[0].ChangePaid,
//                        DiscountAmount = datas[0].DiscountAmount,
//                        OrderDate = DateTime.Now,
//                        ShopId = shopId,
//                        PMID = existPaymentMethod.PMID,
//                        CreateBy = datas[0].CreatedBy,
//                        CreatedDate = DateTime.Now,
//                    };

//                    _context.POS_NDS_Orders.Add(order);
//                    _context.SaveChangesAsync();
//                    // ไม่ต้อง Commit ที่นี่ เพราะจะ Commit ทีเดียวตอนจบ Transaction

//                    // ดึง OrderId ล่าสุด (ควรใช้หลังจาก Commit แต่ในที่นี้เราจะใช้ TransactionScope)
//                    // หมายเหตุ: การดึง OrderId ล่าสุดอาจไม่ปลอดภัยในระบบที่มีการใช้งานพร้อมกัน (Concurrency)
//                    // แนะนำให้ใช้ Auto-increment หรือ Guid สำหรับ OrderId
//                    var lastOrderId = _context.POS_NDS_Orders
//                        .OrderByDescending(o => o.OrderId)
//                        .Select(o => o.OrderId)
//                        .FirstOrDefault();

//                    if (lastOrderId <= 0)
//                    {
//                        throw new Exception("Failed to retrieve the last OrderId.");
//                    }

//                    // เพิ่ม Order Details
//                    foreach (var data in datas)
//                    {
//                        // ดึง Color
//                        var color = _context.POS_NDS_ColorCodes
//                            .FirstOrDefault(w => w.ColorName == data.ColorName);

//                        if (color == null)
//                        {
//                            throw new Exception($"Color '{data.ColorName}' not found for ProductId: {data.ProductId}");
//                        }

//                        // ดึง Variant
//                        var variant = _context.POS_NDS_Variants
//                            .FirstOrDefault(w => w.Size.SizeName == data.SizeName && w.ProductId == data.ProductId && w.ColorId == color.ColorId);

//                        if (variant == null)
//                        {
//                            throw new Exception($"Variant with Size '{data.SizeName}' and Color '{data.ColorName}' not found for ProductId: {data.ProductId}");
//                        }

//                        // สร้าง Order Detail
//                        var orderDetail = new POS_NDS_OrderDetail
//                        {
//                            OrderId = lastOrderId,
//                            VariantId = variant.VariantId,
//                            Qty = data.Quantity,
//                            Price = variant.Price,
//                            TotalAmount = variant.Price * data.Quantity,
//                            Status = 1,
//                            CreateBy = data.CreatedBy,
//                            CreatedDate = DateTime.Now,
//                        };

//                        _context.POS_NDS_OrderDetails.Add(orderDetail);

//                        var existingMasterStock = _context.POS_NDS_MasterStocks.FirstOrDefault(w => w.VariantId == variant.VariantId);
//                        if (existingMasterStock == null)
//                        {
//                            throw new Exception($"dont found Masterstock from variantId : '{variant.VariantId}'");
//                        }
//                        existingMasterStock.Qty = existingMasterStock.Qty - data.Quantity;
//                        existingMasterStock.UpdateBy = data.CreatedBy;
//                        existingMasterStock.UpdatedDate = DateTime.Now;

//                        _context.POS_NDS_MasterStocks.Update(existingMasterStock);

//                        var existShopStock = _context.POS_NDS_StockInShops.FirstOrDefault(
//                                w => w.VariantId == variant.VariantId
//                                && w.ShopId == shopId
//                            );
//                        if (existShopStock == null) { throw new Exception($"not found stockInWarehouse from VariantId: {variant.VariantId}"); }
//                        existShopStock.Qty = existShopStock.Qty - data.Quantity;
//                        existShopStock.UpdateBy = data.CreatedBy;
//                        existShopStock.UpdatedDate = DateTime.Now;

//                        _context.POS_NDS_StockInShops.Update(existShopStock);

//                        //var existingStockInShop = _uow.POS_NDS_StockInShops.GetAll().FirstOrDefault(w => w.ShopId == data.ShopId);
//                        //if(existingStockInShop == null) { throw new Exception($"not found stockInShop from VariantId: {variant.VariantId}"); }
//                        //existingStockInShop.Qty = existingStockInShop.Qty - data.Quantity;
//                        //existingStockInShop.UpdateBy = data.CreatedBy;
//                        //existingStockInShop.UpdatedDate = DateTime.Now;

//                        //_uow.POS_NDS_StockInShops.Update(existingStockInShop);

//                        var stockTransaction = _context.POS_NDS_StockTransactions.FirstOrDefault();

//                        var newTransaction = new POS_NDS_StockTransaction
//                        {
//                            TransactionCode = GenerateStockTransactionId(data.TransactionType, shopId),
//                            TransactionTypeId = 5,
//                            StockInShopId = existShopStock.StockId,
//                            VariantId = variant.VariantId,
//                            Qty = data.Quantity,
//                            OrderId = order.OrderId,
//                            CreateBy = data.CreatedBy,
//                            CreatedDate = DateTime.Now,

//                        };

//                        _context.POS_NDS_StockTransactions.Add(newTransaction);
//                        _context.SaveChangesAsync();
//                    }
//                    var shopName = _context.POS_NDS_MasterShops.Where(s => s.ShopId == shopId).Select(s => s.ShopName);
//                    var shopLogo = _context.POS_NDS_MasterShops.Where(s => s.ShopId == shopId).Select(s => s.LogoURL); ;
//                    // Commit การเปลี่ยนแปลงทั้งหมด
//                    _context.SaveChangesAsync();

//                    // ยืนยัน Transaction
//                    scope.Complete();

//                    return new JsonResult(new { message = "Order added successfully.", orderId = order.OrderId, orderCode = order.OrderCode, shopName = shopName, shopLogo = shopLogo });
//                }
//            }
//            catch (Exception ex)
//            {
//                // หากเกิดข้อผิดพลาด Transaction จะถูก Rollback อัตโนมัติเมื่อ TransactionScope ถูก Dispose
//                return new JsonResult(new { error = $"Error: {ex.Message}" });
//            }
//        }



//        public string GenerateOrderId(int shopId)
//        {
//            // 1. ดึงชื่อร้านและแทน space ด้วย _
//            string nameShop = _context.POS_NDS_MasterShops
//                                 .FirstOrDefault(w => w.ShopId == shopId)?.ShopName ?? "NONE";
//            nameShop = nameShop.Replace(" ", "_");

//            // 2. วันที่
//            string datePart = DateTime.Now.ToString("yyyyMMdd");
//            string searchPattern = $"INV-{nameShop}-{datePart}";

//            // 3. หาลำดับล่าสุด
//            var lastRecord = _context.POS_NDS_Orders
//                                .AsEnumerable()
//                                .Where(o => o.OrderCode.StartsWith(searchPattern))
//                                .OrderByDescending(o => o.OrderId)
//                                .FirstOrDefault();

//            int nextId = 1;
//            if (lastRecord != null)
//            {
//                string[] parts = lastRecord.OrderCode.Split('-');
//                if (parts.Length >= 4 && int.TryParse(parts[3], out int lastId))
//                {
//                    nextId = lastId + 1;
//                }
//            }

//            // 4. สร้าง OrderId
//            return $"INV-{nameShop}-{datePart}-{nextId:D4}";
//        }

//        public string GenerateStockTransactionId(string transactionType, int shopId)
//        {
//            // 1. ดึงชื่อร้านและแทน space ด้วย _
//            string shopName = _context.POS_NDS_MasterShops
//                                 .FirstOrDefault(w => w.ShopId == shopId)?.ShopName ?? "NONE";
//            shopName = shopName.Replace(" ", "_");

//            // 2. Prefix + วันที่
//            string datePart = DateTime.Now.ToString("yyyyMMdd");
//            string prefix = $"{shopName}-{transactionType}-{datePart}-";

//            // 3. หาลำดับล่าสุด
//            var lastRecord = _context.POS_NDS_StockTransactions
//                                .Where(o => o.TransactionCode.StartsWith(prefix))
//                                .OrderByDescending(o => o.TransactionId)
//                                .FirstOrDefault();

//            int nextId = 1;
//            if (lastRecord != null)
//            {
//                string[] parts = lastRecord.TransactionCode.Split('-');
//                if (parts.Length >= 4 && int.TryParse(parts[3], out int lastId))
//                {
//                    nextId = lastId + 1;
//                }
//            }

//            // 4. สร้าง TransactionId
//            return $"{shopName}-{transactionType}-{datePart}-{nextId:D4}";
//        }



//        public class CreateOrderTransactionRequestDTO
//        {
//            public string ColorName { get; set; } = string.Empty;
//            public string SizeName { get; set; } = string.Empty;
//            public int ProductId { get; set; }
//            public int Quantity { get; set; }
//            public string TransactionType { get; set; } = string.Empty;
//            public string PaymentMethod { get; set; } = string.Empty;
//            public decimal TotalAmount { get; set; }
//            public decimal ChangePaid { get; set; }
//            public decimal ReceivePaid { get; set; }
//            public decimal DiscountAmount { get; set; }
//            public string CreatedBy { get; set; } = string.Empty;

//        }
//    }
//}
