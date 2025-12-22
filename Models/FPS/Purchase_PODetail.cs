using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RFIDApi.Models.FPS
{
    [Table("Purchase_PODetail")]

    public class Purchase_PODetail
    {
        public string PRNo { get; set; }
        public int? ItemNo { get; set; }
        public string? DlvCode { get; set; }
        public string? CustomerPO { get; set; }
        public int? SizeId { get; set; }
        public string? Size { get; set; }
        public string? PONo { get; set; }
        public string? OrderNo { get; set; }
        public string? ItemId { get; set; }
        public string? ItemDesc { get; set; }
        public string? ItemCode { get; set; }
        public string? ColorCode { get; set; }
        public double? PRQty { get; set; }
        public double? POQty { get; set; }
        public string? UOM { get; set; }
        public double? UnitPrice { get; set; }
        public string? PriceCurrency { get; set; }
        public DateTime? NeedDate { get; set; }
        public DateTime? FinalETA { get; set; }
        public string? OrderCfmNo { get; set; }
        public DateTime? LastUpdateETA { get; set; }
        public string? LastSupComment { get; set; }
        public double? ReceiveQty { get; set; }
        public DateTime? LastReceive { get; set; }
        public string? VendorId { get; set; }
        public string? SupplierDetail { get; set; }
        public string? ItemRemark { get; set; }
        public string? ChargeDetail { get; set; }
        public string? ChargeType { get; set; }
        public string? ChargeValueType { get; set; }
        public decimal? ChargeValue { get; set; }
        public double? ItemAmount { get; set; }
        public double? ItemNetAmount { get; set; }
    }
}
