using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace RFIDApi.Models
{
    [Table("RFID_StockTransaction")]
    public class PrinterReceipt
    {
        [Key]
        public int TransactionId { get; set; } // Primary Key

        [Required]
        public int ProductId { get; set; } // Foreign Key referencing Product table

        [Required]
        [StringLength(50)]
        public string? TransactionType { get; set; } // Type of transaction (e.g., Receive, Sale, Transfer)

        [Required]
        public int Quantity { get; set; } // Quantity of product in the transaction

        [DataType(DataType.DateTime)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime TransactionDate { get; set; } = DateTime.Now; // Default to current date

        [StringLength(100)]
        public string? Reference { get; set; } // Reference for the transaction (e.g., Invoice number)

        [StringLength(255)]
        public string? Remark { get; set; } // Additional remarks for the transaction

    }
}