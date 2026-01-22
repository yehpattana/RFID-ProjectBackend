using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations; // For Data Annotations
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore; // For Relationships
namespace RFIDApi.Models
{
    [Table("Shopify_LinkRFID")]
    public class Product
    {
        [Key]
        [Required]
        public string Sku {  get; set; }
        public string? Style { get; set; }
        public string? Color { get; set; }
        public string? Size { get; set; }
        public string? CustomerStyle { get; set; }
        public decimal Price { get; set; }
        public string? Barcode { get; set; }
        public int Shopify_InventoryQty { get; set; }
        public int RFIDScan { get; set; }
        public ICollection<ProductRFID>? productRFIDs { get; set; }

    }
}