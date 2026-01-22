
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace RFIDApi.Models
{
    [Table("POS_NDS_Unit")]
    public class POS_NDS_Unit
    {
        [Key]
        public int UnitId { get; set; }

        [Required]
        public string? UnitName { get; set; }

        public string? Description { get; set; }

        public int Status { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public string? CreateBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string UpdateBy { get; set; } = "Not Updated";

        public DateTime? Delete_At { get; set; }

        // Navigation Property
        public virtual ICollection<POS_NDS_Variant>? Variants { get; set; }
    }
}