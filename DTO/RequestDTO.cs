namespace RFIDApi.DTO
{
    public class RequestDTO
    {
    }

    public class AddRfidToProductRequest
    {
        public string Barcode { get; set; }
        public string Style { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public int targetQty { get; set; }
        public string EPC { get; set; }
    }

    public class DeleteRfidProductRequest
    {
        public string sku { get; set; }
        
        public string rfid { get; set; }

    }
}
