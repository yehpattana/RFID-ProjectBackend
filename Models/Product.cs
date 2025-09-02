using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations; // For Data Annotations
using System.ComponentModel.DataAnnotations.Schema; // For Relationships
namespace RFIDApi.Models
{
    [Table("RFID_Product")]
    public class Product
    {
        [Key] // Primary Key
        public int ProductId { get; set; }

        [Required] 
        [StringLength(50)] 
        public string? ProductCode { get; set; }

        [Required]
        [StringLength(200)]
        public string? ProductName { get; set; }

        public string? RFIDData { get; set; }

        public int CategoryId { get; set; }

        [Required]
        [DataType(DataType.Currency)] 
        public decimal UnitPrice { get; set; }

        public string? SKUCode { get; set; }
        public string? Color { get; set; }

        public string? Size { get; set; }

        public int QuantityInStock { get; set; }

        public bool IsActive { get; set; }

        public int Status { get; set; }

        public string? Remark { get; set; }

        [DataType(DataType.DateTime)] // Date Format
        public DateTime CreateDate { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime UpdateDate { get; set; }

    }
}