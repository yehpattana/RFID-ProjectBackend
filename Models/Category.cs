using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RFIDApi.Models
{
    [Table("RFID_Category")]
    public class Category
    {
        [Key]
        public int CategoryId { get; set; } // Primary Key

        [Required]
        [StringLength(100)]
        public string? CategoryName { get; set; } // ชื่อหมวดหมู่สินค้า

        [StringLength(500)]
        public string? Description { get; set; } // คำอธิบายหมวดหมู่

        [DataType(DataType.DateTime)]
        [Required]
        public DateTime CreateDate { get; set; } // วันที่สร้างหมวดหมู่

        [DataType(DataType.DateTime)]
        public DateTime UpdateDate { get; set; } // วันที่แก้ไขล่าสุด (ค่าเป็น null ได้)

    }
}