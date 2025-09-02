
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace RFIDApi.Models
{
    [Table("POS_NDS_TransactionType")]
    public class POS_NDS_TransactionType
    {
        [Key]
        public int TransactionTypeId { get; set; }

        [Required]
        public string? TransactionName { get; set; }

        public string? Remark { get; set; }

        public int Status { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public string? CreateBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string UpdateBy { get; set; } = "Not Updated";

        public DateTime? Delete_At { get; set; }

        // Navigation Property
        public virtual ICollection<POS_NDS_StockTransaction>? StockTransactions { get; set; }
    }
}
