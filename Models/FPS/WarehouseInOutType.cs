using System.ComponentModel.DataAnnotations.Schema;

namespace RFIDApi.Models.FPS
{
    [Table("Warehouse_InoutType")]
    public class WarehouseInOutType
    {
        public string InoutType { get; set; }
        public string TranType { get; set; }
        public string TypeName { get; set; }
        public bool TypeAdmin { get; set; }
    }
}
