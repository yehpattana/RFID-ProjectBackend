
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace RFIDApi.Models
{
    [Table("POS_NDS_MasterStock")]
    public class POS_NDS_MasterStock
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StockId { get; set; }

        [ForeignKey("Variant")]
        public int VariantId { get; set; }

        public int MaxQty { get; set; }

        public int MinQty { get; set; }

        public int Qty { get; set; }

        public int Status { get; set; }

        public string? Remark { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public string? CreateBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string UpdateBy { get; set; } = "Not Updated";

        public DateTime? Delete_At { get; set; }

        // Navigation Property
        public virtual POS_NDS_Variant? Variant { get; set; }
    }
}