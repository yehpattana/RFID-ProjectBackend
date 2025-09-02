
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RFIDApi.Models
{
    [Table("POS_NDS_Product")]
    public class POS_NDS_Product
    {
        [Key]
        public int ProductId { get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        [ForeignKey("Brand")]
        public int BrandId { get; set; }

        [Required]
        public string ProductName { get; set; } = string.Empty;

        public string ProductDescription { get; set; } = string.Empty;

        public string Remark { get; set; } = string.Empty;

        public int Status { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public string CreateBy { get; set; } = string.Empty;

        public DateTime? UpdatedDate { get; set; }

        public string UpdateBy { get; set; } = "Not Updated";

        public DateTime? Delete_At { get; set; }

        // Navigation Properties
        public virtual POS_NDS_Category? Category { get; set; }
        public virtual POS_NDS_Brand? Brand { get; set; }
        public virtual ICollection<POS_NDS_Variant>? Variants { get; set; }

        public virtual ICollection<POS_NDS_Style>? Styles { get; set; }
        public virtual ICollection<POS_NDS_Image>? Images { get; set; }
    }
}
