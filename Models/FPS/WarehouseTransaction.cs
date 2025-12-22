using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using Microsoft.EntityFrameworkCore;

namespace RFIDApi.Models.FPS
{
    [Table("Warehouse_Transection")]

    public class FPSWarehouseTransection
    {
        public string ReceiveNo { get; set; }          // [ReceiveNo]
        public string Warehouse { get; set; }         // [Warehouse]
        public string RFId { get; set; }              // [RFId]
        public string CompanyCode { get; set; }       // [CompanyCode]
        public string PONo { get; set; }              // [PONo]
        public int? POItemNo { get; set; }          // [POItemNo]
        public string? OrderNo { get; set; }           // [OrderNo]
        public string? UOM { get; set; }               // [UOM]
        public string? InType { get; set; }            // [InType]
        public DateTime? ReceiveDate { get; set; }    // [ReceiveDate]
        public bool OutStatus { get; set; }         // [OutStatus]
        public DateTime? OutDate { get; set; }        // [OutDate]
        public string? OutType { get; set; }           // [OutType]
        public string? OutNo { get; set; }             // [OutNo]
        public string? IssueRemark { get; set; }       // [IssueRemark]
        public string? InputBy { get; set; }           // [InputBy]
        public DateTime? InputDate { get; set; }      // [InputDate]
        public string? MoveToWH { get; set; }          // [MoveToWH]
        public string? TranNoMove { get; set; }        // [TranNoMove]
        public string? InputOutBy { get; set; }        // [InputOutBy]
        public DateTime? InputOutDate { get; set; }   // [InputOutDate]

        // Navigation
        public virtual WarehouseRFID WarehouseRFID { get; set; }   // a -> b
        public virtual WarehouseReceive WarehouseReceive { get; set; } // a -> d
    }
}
