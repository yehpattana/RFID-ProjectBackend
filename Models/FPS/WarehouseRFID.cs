using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;

namespace RFIDApi.Models.FPS
{
    [Table("Warehouse_RFId")]
    public class WarehouseRFID
    {
        [Key]
        [Required]
        public string RFID { get; set; }               // [RFID]
        public string? ReceiveNo { get; set; }          // [ReceiveNo]
        public string? PONo { get; set; }               // [PONo]
        public int? POItemNo { get; set; }           // [POItemNo]
        public string? UOM { get; set; }                // [UOM]
        public DateTime? CreateDate { get; set; }      // [CreateDate]
        public string? CompanyCode { get; set; }        // [CompanyCode]
        public string? ItemCode { get; set; }           // [ItemCode]
        public string? ColorCode { get; set; }          // [ColorCode]
        public string? Size { get; set; }               // [Size]
        public string? SKU { get; set; }                // [SKU]
        public DateTime? FirstInDate { get; set; }     // [FirstInDate]

        public virtual MasterProductOnline MasterProductOnline { get; set; } // b -> c
        public ICollection<FPSWarehouseTransection> WarehouseTransections { get; set; }
    }
}
