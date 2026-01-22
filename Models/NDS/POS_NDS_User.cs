
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace RFIDApi.Models
{
    [Table("POS_NDS_User")]
    public class POS_NDS_User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public int RoleId { get; set; }
        public int? ShopId { get; set; }
        public int Status {  get; set; }
        public string? Remark { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string? CreateBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdateBy { get; set; } = "Not Updated";
        public DateTime? Delete_At { get; set; }
        // Navigation Property
        public virtual POS_NDS_Role? Roles { get; set; }
        public virtual POS_NDS_MasterShop? Shop { get; set; }
    }
}
