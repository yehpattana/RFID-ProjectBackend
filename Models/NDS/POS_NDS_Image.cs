
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace RFIDApi.Models
{
    [Table("POS_NDS_Image")]
    public class POS_NDS_Image
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ImageId { get; set; }

        [ForeignKey("Variant")]
        public int? VariantId { get; set; } = null;

        [ForeignKey("Product")]
        public int? ProductId { get; set; } = null;

        [Required]
        public string? ImageUrl { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string UpdateBy { get; set; } = "Not Updated";

        public DateTime? Delete_At { get; set; }

        // Navigation Property
        public virtual POS_NDS_Variant? Variant { get; set; }
        public virtual POS_NDS_Product? Product { get; set; }
    }
}