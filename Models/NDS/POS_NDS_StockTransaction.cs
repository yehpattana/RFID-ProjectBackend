
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace RFIDApi.Models
{
    [Table("POS_NDS_StockTransaction")]
    public class POS_NDS_StockTransaction
    {
        [Key]
        public int TransactionId { get; set; } // ไม่ใช้ [increment] เพราะเป็น varchar

        public string? TransactionCode { get; set; }
        [ForeignKey("Variant")]
        public int VariantId { get; set; }

        [ForeignKey("Warehouse")]
        public string? WarehouseId { get; set; }

        [ForeignKey("StockInShop")]
        public string? StockInShopId { get; set; } // Nullable

        [ForeignKey("Order")]
        public int? OrderId { get; set; }

        [ForeignKey("TransactionType")]
        public int TransactionTypeId { get; set; } // 1 = IN, 2 = OUT, 3 = RECEIVE_TRANSFER, 4 = TRANSFER, 5 = SOLD_OUT, 6 = ADJUST

        [Required]
        [Range(0, int.MaxValue)]
        public int Qty { get; set; }

        public string? Reason { get; set; }

        [ForeignKey("StatusTransaction")]
        public int? TrxStatus { get; set; } = null;
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public string? CreateBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string UpdateBy { get; set; } = "Not Updated";

        public DateTime? Delete_At { get; set; }

        // Navigation Properties
        public virtual POS_NDS_Variant? Variant { get; set; }
        public virtual POS_NDS_Warehouse? Warehouse { get; set; }
        public virtual POS_NDS_StockInShop? StockInShop { get; set; }
        public virtual POS_NDS_Order? Order { get; set; }
        public virtual POS_NDS_TransactionType? TransactionType { get; set; }
        public virtual POS_NDS_StatusTransaction? StatusTransaction { get; set; }
    }
}
