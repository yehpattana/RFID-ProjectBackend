
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace RFIDApi.Models
{
    [Table("POS_NDS_Order")]
    public class POS_NDS_Order
    {
        [Key]
        public int OrderId { get; set; }

        public string? OrderCode { get; set; }

        [ForeignKey("Shop")]
        public int ShopId { get; set; }

        public string? CustomerId { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.Now;

        public decimal TotalAmount { get; set; }
        
        public decimal? ReceivedAmount {  get; set; }
        public decimal? ChangeAmount { get; set; }
        public decimal? DiscountAmount { get; set; }
        public int PMID { get; set; }

        public int Status { get; set; } // 0 = Pending, 1 = Paid, 2 = Shipped, 3 = Canceled

        public string? Remark { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public string? CreateBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string UpdateBy { get; set; } = "Not Updated";

        public DateTime? Delete_At { get; set; }

        // Navigation Properties
        public virtual POS_NDS_MasterShop? Shop { get; set; }

        public virtual POS_NDS_PaymentMethod? Payments { get; set; }
        public virtual ICollection<POS_NDS_OrderDetail>? OrderDetails { get; set; }
        public virtual ICollection<POS_NDS_StockTransaction>? StockTransactions { get; set; }
    }
}
