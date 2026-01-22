namespace RFIDApi.DTO.Data
{
    public class PODetailDTO
    {
        public string ItemCode { get; set; }
        public int? ItemNo { get; set; }
        public string ColorCode { get; set; }

        public string SKU { get; set; }
        public string size { get; set; }
        public string ProductBarcode { get; set; }

        public string UOM { get; set; }

        public double POQty { get; set; }
        public double BalanceQty { get; set; }
    }
}
