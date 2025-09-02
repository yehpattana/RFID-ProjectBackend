using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RFIDApi.Models
{
    [Table("RFID_SalesDetail")]
    public class SalesDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SalesDetailId { get; set; } // Primary Key with Identity

        [Required]
        public int SalesId { get; set; } // ใบเสร็จการขาย (FK)

        [Required]
        public int ProductId { get; set; } // รหัสสินค้า (FK)

        [Required]
        public int Quantity { get; set; } // จำนวนขาย

        [Required]
        public decimal UnitPrice { get; set; } // ราคาขายต่อหน่วย


        public decimal Discount { get; set; } = 0; // ส่วนลด (Default = 0)


        public decimal TotalPrice { get; private set; } // รวมราคา (คำนวณอัตโนมัติ)

    }
}