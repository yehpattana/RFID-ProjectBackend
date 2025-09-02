
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace RFIDApi.Models
{
    [Table("POS_NDS_StatusTransaction")]
    public class POS_NDS_StatusTransaction
    {
        [Key]
        public int TrxStatusId { get; set; }


        public string? TrxName { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public string? CreateBy { get; set; }

        // Navigation Property
        public virtual ICollection<POS_NDS_StockTransaction>? StockTransaction { get; set; }
    }
}
