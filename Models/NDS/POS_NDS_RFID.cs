
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace RFIDApi.Models
{
    [Table("POS_NDS_RFID")]
    public class POS_NDS_RFID
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RFID_Id { get; set; }

        [Required]
        public string? RFIDCode { get; set; }

        [ForeignKey("Variant")]
        public int VariantId { get; set; }

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
