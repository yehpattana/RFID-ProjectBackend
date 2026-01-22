using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RFIDApi.Models
{
    [Table("Shopify_WarehouseStock")]
    public class ShopifyWarehouseStock
    {
        [Key]
        [Required]
        public long LocationId { get; set; }
        public string Warehouse { get; set; }
        public string Address { get; set; }
        public int AvailableStock { get; set; }
    }
}
