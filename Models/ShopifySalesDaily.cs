using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations; // For Data Annotations
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore; // For Relationships
namespace RFIDApi.Models
{
    [Table("ShopifySales_Daily_BySource_Size_SKU")]
    [Keyless]
    public class ShopifySalesDaily
    {
        public DateTime? order_date {  get; set; }
        public string? source_name { get; set; }
        public string? currency { get; set; }
        public string? sku {  get; set; }
        public string? title { get; set; }
        public string? size { get; set; }
        public int? total_qty { get; set; }
        public decimal? gross_sales { get; set; }
        public decimal? total_discount { get; set; }
        public decimal? net_sales { get; set; }
        public decimal? total_taxes { get; set; }
        public int? distinct_orders { get; set; }
    }
}