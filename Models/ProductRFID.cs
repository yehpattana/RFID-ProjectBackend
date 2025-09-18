using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations; // For Data Annotations
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore; // For Relationships
namespace RFIDApi.Models
{
    [Table("Shopify_RFID_Product")]
    public class ProductRFID
    {
        [Required]
        public string SKU {  get; set; }
        [Required]
        public string RFID { get; set; }
        public DateTime CreateDate { get; set; }

        public virtual Product Product { get; set; }

    }
}