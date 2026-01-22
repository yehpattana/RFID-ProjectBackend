using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;

namespace RFIDApi.Models.FPS
{
    [Table("Master_ProductOnline")]
    public class MasterProductOnline
    {
        public string CompanyCode { get; set; }
        public string ItemCode { get; set; }
        public string ColorCode { get; set; }
        public string Size { get; set; }
        public string ProductName { get; set; }
        public string SKU { get; set; }
        public string? ProductBarcode { get; set; }
        public bool UseGS1 { get; set; }
        public string? UOM { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string EditBy { get; set; }
        public DateTime EditDate { get; set; }

        public ICollection<WarehouseRFID> WarehouseRFIDs{ get; set; }
    }
}
