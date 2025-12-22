using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;

namespace RFIDApi.Models.FPS
{
    [Table("Warehouse_Receive")]
    public class WarehouseReceive
    {
        [Key]
        [Required]
        public string ReceiveNo { get; set; }           // [ReceiveNo] varchar/nvarchar
        public DateTime? ReceiveDate { get; set; }      // [ReceiveDate] datetime
        public string? ReceiveType { get; set; }         // [ReceiveType] varchar/nvarchar
        public string? CompanyCode { get; set; }         // [CompanyCode] varchar/nvarchar
        public string? DeliveryNo { get; set; }          // [DeliveryNo] varchar/nvarchar
        public string? InvoiceNo { get; set; }           // [InvoiceNo] varchar/nvarchar
        public DateTime? InvoiceDate { get; set; }      // [InvoiceDate] datetime
        public string? Remark { get; set; }              // [Remark] varchar/nvarchar
        public string? Warehouse { get; set; }           // [Warehouse] varchar/nvarchar
        public string? InputBy { get; set; }             // [InputBy] varchar/nvarchar
        public DateTime? InputDate { get; set; }        // [InputDate] datetime
        public string? EditBy { get; set; }              // [EditBy] varchar/nvarchar
        public DateTime? EditDate { get; set; }         // [EditDate] datetime

        public ICollection<FPSWarehouseTransection> WarehouseTransections { get; set; }
    }
}
