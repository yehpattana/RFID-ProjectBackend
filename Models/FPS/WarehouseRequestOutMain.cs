using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RFIDApi.Models.FPS
{
    [Table("Warehouse_RequestOutMain")]
    public class WarehouseRequestOutMain
    {
        [Key]
        [Required]
        public string OutNo { get; set; } = null!;          // nvarchar(20) NOT NULL

        public DateTime RequestDate { get; set; }           // datetime NOT NULL
        public string RequestBy { get; set; } = null!;      // nvarchar(50) NOT NULL

        public string OutType { get; set; } = null!;        // nvarchar(20) NOT NULL

        public string CreateBy { get; set; } = null!;       // nvarchar(50) NOT NULL
        public DateTime CreateDate { get; set; }            // datetime NOT NULL

        public string EditBy { get; set; } = null!;         // nvarchar(50) NOT NULL
        public DateTime EditDate { get; set; }              // datetime NOT NULL
    }
}
