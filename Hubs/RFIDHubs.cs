using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using RFIDReaderAPI;
using System.Diagnostics;
using RFIDApi.Models.Context; // Namespace ของ RFIDReaderAPI.dll (สมมติ)

namespace RFIDApi.Hubs
{
    public class RFIDHubs : Hub
    {
        private readonly RFIDDbContext _db;
        private readonly IHubContext<RFIDHubs> _hubContext;
        public RFIDHubs(RFIDDbContext db, IHubContext<RFIDHubs> hubContext)
        {
            _db = db;
            _hubContext = hubContext;
        }

        // เมธอดสำหรับส่งข้อมูล RFID แบบ real-time
        public async Task SendRFIDUpdate(string tagId)
        {
            if (Clients == null)
            {
                Debug.WriteLine("Clients is null, cannot send message.");
                return;
            }
            Debug.WriteLine($"SendRFIDUpdate is Work {tagId}.");
            var rfidTag = new
            {
                EPC = tagId,
                ReadTime = DateTime.UtcNow,
                IsActive = 1
            };


            // ส่งข้อมูลไปยัง client ที่เชื่อมต่อทั้งหมด
            await Clients.All.SendAsync("ReceiveRFIDUpdate", tagId);
        }

        // เมธอดสำหรับดึง RFID tags ล่าสุด
        //public async Task GetLatestRFIDTags()
        //{
        //    var tags = await _db.RFIDTags
        //        .Take(10)
        //        .ToListAsync();

        //    // ส่งข้อมูลไปยัง client ที่เรียกเมธอดนี้
        //    await Clients.Caller.SendAsync("ReceiveLatestRFIDTags", tags);
        //}
        

        // เมธอดสำหรับอ่าน RFID แบบ real-time (ตัวอย่าง)
        //public async Task StartReading()
        //{
        //    try
        //    {
        //        while (true) // ควรใช้ background task ใน Production
        //        {
        //            string tagId = _rfidReader.ReadTag(); // อ่านจาก RFIDReaderAPI.dll (ปรับตามจริง)
        //            if (!string.IsNullOrEmpty(tagId))
        //            {
        //                await SendRFIDUpdate(tagId, "Reader1");
        //            }
        //            await Task.Delay(1000); // รอ 1 วินาทีก่อนอ่านครั้งถัดไป
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        await Clients.Caller.SendAsync("Error", ex.Message);
        //    }
        //}
        //public async Task TestBroadcast()
        //{
        //    var testTag = new RFIDTag
        //    {
        //        EPC = "TEST123456789",
        //        ReadTime = DateTime.UtcNow,
        //        IsActive = 1
        //    };

        //    await Clients.All.SendAsync("ReceiveRFIDUpdate", testTag);
        //}
    }
}