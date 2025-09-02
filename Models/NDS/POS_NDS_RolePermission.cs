
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
namespace RFIDApi.Models
{
    [Table("POS_NDS_RolePermission")]
    [PrimaryKey(nameof(RoleId), nameof(PermissionId))]
    public class POS_NDS_RolePermission
    {
        public int RoleId { get; set; }

        public int PermissionId { get; set; }


        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public string? CreateBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string UpdateBy { get; set; } = "Not Updated";

        public DateTime? Delete_At { get; set; }

        // Navigation Property
        public virtual POS_NDS_Role? Roles { get; set; }
        public virtual POS_NDS_Permission? Permissions { get; set; }
    }
}
