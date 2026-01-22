
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace RFIDApi.Models
{
    [Table("POS_NDS_Warehouse_Shelf")]
    public class POS_NDS_Warehouse_Shelf
    {
        [Key]
        public int ShelfId { get; set; }

        [ForeignKey("Warehouse")]
        public string? WarehouseId { get; set; } // เปลี่ยนจาก varchar เป็น int เพื่อให้สอดคล้องกับการใช้งานทั่วไป

        [Required]
        public string? ShelfName { get; set; }

        public string? ZoneCode { get; set; }

        public string? LocationCode { get; set; }

        public string? Remark { get; set; }

        public int Status { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public string? CreateBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string UpdateBy { get; set; } = "Not Updated";

        public DateTime? Delete_At { get; set; }

        // Navigation Properties
        public virtual POS_NDS_Warehouse? Warehouse { get; set; }
        public virtual ICollection<POS_NDS_WarehouseStock>? WarehouseStocks { get; set; }
    }
}
