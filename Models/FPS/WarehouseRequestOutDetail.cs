using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RFIDApi.Models.FPS
{
    [Table("Warehouse_RequestOutDetail")]
    public class WarehouseRequestOutDetail
    {
        
        [Required]
        public string OutNo { get; set; } = null!;        // nvarchar(20) NOT NULL
        [Required]
        public string ItemCode { get; set; } = null!;     // nvarchar(30) NOT NULL
        [Required]
        public string ColorCode { get; set; } = null!;    // nvarchar(50) NOT NULL
        [Required]
        public string Size { get; set; } = null!;         // nvarchar(15) NOT NULL
        public int OutQty { get; set; }                    // int NOT NULL
        public string UOM { get; set; } = null!;           // nvarchar(10) NOT NULL
        public bool OutStatus { get; set; }                // bit NOT NULL
    }
}
