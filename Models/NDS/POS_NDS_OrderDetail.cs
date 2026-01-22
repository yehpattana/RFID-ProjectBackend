
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace RFIDApi.Models
{
    [Table("POS_NDS_OrderDetail")]
    public class POS_NDS_OrderDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderDetailId { get; set; }

        [ForeignKey("Order")]
        public int OrderId { get; set; }

        [ForeignKey("Variant")]
        public int VariantId { get; set; }

        public int Qty { get; set; }

        public decimal Price { get; set; }

        public decimal TotalAmount { get; set; }

        public int Status { get; set; } // 0 = Pending, 1 = Processed, 2 = Canceled

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public string? CreateBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string UpdateBy { get; set; } = "Not Updated";

        public DateTime? Delete_At { get; set; }

        // Navigation Properties
        public virtual POS_NDS_Order? Order { get; set; }
        public virtual POS_NDS_Variant? Variant { get; set; }
    }
}
