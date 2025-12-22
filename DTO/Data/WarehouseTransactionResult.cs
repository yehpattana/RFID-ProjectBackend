namespace RFIDApi.DTO.Data
{
    public class ProductTransactionResult
    {
        public string Warehouse { get; set; }
        public string RFId { get; set; }
        public string ProductBarcode { get; set; }
        public string SKU { get; set; }
        public string ItemCode { get; set; }
        public string ColorCode { get; set; }
        public string Size { get; set; }
        public string ReceiveNo { get; set; }
        public DateTime? ReceiveDate { get; set; }
        public string InType { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public string PONo { get; set; }
        public string UOM { get; set; }
        public bool OutStatus { get; set; }
        public DateTime? OutDate { get; set; }
        public string OutNo { get; set; }
        public string InputBy { get; set; }
        public DateTime? InputDate { get; set; }
    }
}
