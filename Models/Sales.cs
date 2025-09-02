using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace RFIDApi.Models
{
    [Table("RFID_Sales")]
    public class Sales
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SalesId { get; set; } // Primary Key with Identity

        [Required]
        public DateTime SalesDate { get; set; } = DateTime.Now; // วันที่ขาย (Default = ปัจจุบัน)

        [Required]
        public decimal TotalAmount { get; set; } // ยอดรวมทั้งหมด

        [StringLength(50)]
        public string? PaymentMethod { get; set; } // วิธีชำระเงิน (เงินสด, บัตร, QR)

        public int CustomerId { get; set; } // ลูกค้า (ถ้ามี) (FK)

        [StringLength(255)]
        public string? Remark { get; set; } // หมายเหตุ

        [Required]
        public DateTime CreateDate { get; set; } = DateTime.Now; // วันที่สร้าง (Default = ปัจจุบัน)

    }
}