
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace RFIDApi.Models
{
    [Table("POS_NDS_Style")]
    public class POS_NDS_Style
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StyleId { get; set; }

        public int ProductId { get; set; }
        public string? StyleName { get; set; }
        public string? StyleDescription { get; set; }
        public string? StyleCategory { get; set; }
        public string? StyleCode { get; set; }
        public int Status { get; set; }
        public string? Remark { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string? CreateBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string UpdateBy { get; set; } = "Not Updated";

        public DateTime? Delete_At { get; set; }

        public virtual POS_NDS_Product? POS_NDS_Product { get; set; }
    }
}
