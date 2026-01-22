
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace RFIDApi.Models
{
    [Table("POS_NDS_VariantStyle")]
    public class POS_NDS_VariantStyle
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VariantStyleId { get; set; }
        public int StyleId { get; set; }
        public int VariantId { get; set; }
        public string? Remark { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string? CreateBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string UpdateBy { get; set; } = "Not Updated";

        public DateTime? Delete_At { get; set; }

        public virtual POS_NDS_Variant? Variant { get; set; }
        public virtual POS_NDS_Style? Style { get; set; }
    }
}
