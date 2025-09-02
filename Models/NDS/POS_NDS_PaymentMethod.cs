
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace RFIDApi.Models
{
    [Table("POS_NDS_PaymentMethod")]
    public class POS_NDS_PaymentMethod
    {
        [Key]
        public int PMID { get; set; }

        public string PMNAME { get; set; }


        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public string? CreateBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string UpdateBy { get; set; } = "Not Updated";

        public DateTime? Delete_At { get; set; }

        // Navigation Properties

        public virtual ICollection<POS_NDS_Order>? Orders { get; set; }
    }
}
