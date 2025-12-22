using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RFIDApi.Models.FPS
{
    [Table("Master_Warehouse")]
    public class MasterWarehouse
    {
        [Key]
        [Required]
        public string Warehouse { get; set; }
        public string WarehouseDesc { get; set; }
        public string CompanyCode { get; set; }
        public bool Cancel { get; set; }
        public string EditBy { get; set; }
        public DateTime EditDate { get; set; }
    }
}
