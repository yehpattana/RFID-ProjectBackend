
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace RFIDApi.Models
{
    [Table("POS_NDS_MasterShop")]
    public class POS_NDS_MasterShop
    {
        [Key]
        public int ShopId { get; set; }

        [Required]
        public string? ShopName { get; set; }

        public string? Remark { get; set; }

        public int Status { get; set; }

        public string LogoURL { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public string? CreateBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string UpdateBy { get; set; } = "Not Updated";

        public DateTime? Delete_At { get; set; }

        // Navigation Properties
        public virtual ICollection<POS_NDS_Order>? Orders { get; set; }
        public virtual ICollection<POS_NDS_StockInShop>? StockInShops { get; set; }

        public virtual ICollection<POS_NDS_User>? Users { get; set; }
        public virtual ICollection<POS_NDS_UserShop>? UserShops { get; set; }
    }
}
