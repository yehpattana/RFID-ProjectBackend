
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace RFIDApi.Models
{
    [Table("POS_NDS_Size")]
    public class POS_NDS_Size
    {
        [Key]
        public int SizeId { get; set; }

        [Required]
        public string? SizeName { get; set; }

        public string? Remark { get; set; }

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