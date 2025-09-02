using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing.Printing;
using System.Drawing;


namespace RFIDApi.Printer
{
    public class PrinterReceipt
    {
        public PrinterReceipt() { }
        public void PrintReceipt()
        {
            PrintDocument pd = new PrintDocument();
            pd.PrinterSettings.PrinterName = "IHR810"; // ชื่อ printer ที่ติดตั้ง

            // กำหนดขนาดกระดาษ (เช่น 80mm x 297mm = 3.15" x 11.7")
            pd.DefaultPageSettings.PaperSize = new PaperSize("Custom", 315, 1170); // หน่วย: 1/100 inch

            pd.PrintPage += (sender, e) =>
            {
                float y = 0;
                Font font = new Font("Consolas", 10); // ใช้ฟอนต์ที่พิมพ์ง่ายและไม่เพี้ยน

                e.Graphics.DrawString("NDS Shop", font, Brushes.Black, 10, y); y += 20;
                e.Graphics.DrawString("ใบเสร็จรับเงิน", font, Brushes.Black, 10, y); y += 20;
                e.Graphics.DrawString("------------------------", font, Brushes.Black, 10, y); y += 20;
                e.Graphics.DrawString("ลาเต้ x1          60.00", font, Brushes.Black, 10, y); y += 20;
                e.Graphics.DrawString("เอสเพรสโซ่ x1      45.00", font, Brushes.Black, 10, y); y += 20;
                e.Graphics.DrawString("------------------------", font, Brushes.Black, 10, y); y += 20;
                e.Graphics.DrawString("รวมทั้งหมด     105.00", font, Brushes.Black, 10, y); y += 20;
            };

            pd.Print(); // พิมพ์ทันที
        }
    }
}