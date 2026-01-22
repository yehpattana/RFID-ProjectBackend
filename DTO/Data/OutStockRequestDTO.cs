namespace RFIDApi.DTO.Data
{
    public class RequestOutHeader
    {
        public string RequestNo { get; set; } = string.Empty; // auto / generate
        public DateTime RequestDate { get; set; }
        public string OutType { get; set; } = string.Empty;   // from Warehouse_InoutType
        public string RequestBy { get; set; } = string.Empty; // user login
        public string? OutsourcePONo { get; set; }             // optional (POType = 3)
    }
    public class RequestOutItem
    {
        public string ProductCode { get; set; } = string.Empty;
        public string? Color { get; set; }
        public string? Size { get; set; }
        public decimal Qty { get; set; }
        public string Uom { get; set; } = string.Empty;
    }
    public class OutstockRequest
    {
        public RequestOutHeader Header { get; set; } = new();
        public List<RequestOutItem> Items { get; set; } = new();
    }

    public class CheckRequestOutstock
    {
        public string ItemCode { get; set; } = string.Empty;
        public string ColorCode { get; set; } = string.Empty;
        public string Size { get; set; } = string.Empty;

    }
}
