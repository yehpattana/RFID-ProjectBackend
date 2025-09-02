
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace RFIDApi.Models
{
    [Table("POS_NDS_Warehouse")]
    public class POS_NDS_Warehouse
    {
        [Key]
        public string? WarehouseId { get; set; }

        [Required]
        public string? WarehouseName { get; set; }

        public int Status { get; set; }

        public string? Remark { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public string? CreateBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string UpdateBy { get; set; } = "Not Updated";

        public DateTime? Delete_At { get; set; }

        // Navigation Properties
        public virtual ICollection<POS_NDS_WarehouseStock>? WarehouseStocks { get; set; }
        public virtual ICollection<POS_NDS_StockTransaction>? StockTransactions { get; set; }
        public virtual ICollection<POS_NDS_Warehouse_Shelf>? Shelves { get; set; }
    }
}
