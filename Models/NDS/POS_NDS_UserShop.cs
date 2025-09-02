
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace RFIDApi.Models
{
    [Table("POS_NDS_UserShop")]
    public class POS_NDS_UserShop
    {
        [Key]
        public int UserId { get; set; }

        public int ShopId { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public string? CreateBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string UpdateBy { get; set; } = "Not Updated";

        public DateTime? Delete_At { get; set; }

        // Navigation Property
        public virtual POS_NDS_User? Users { get; set; }

        public virtual POS_NDS_MasterShop? Shops { get; set; }
    }
}
