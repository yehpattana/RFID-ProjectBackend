namespace RFIDApi.DTO.Data
{
    public class CreateWarehouseReceiveInDTO
    {
        public string receiveNo { get; set; }
        public DateTime? receiveDate { get; set; }
        public string? receiveType { get; set; }
        public string? companyCode { get; set; }
        public string? deliveryNo { get; set; }
        public string? invoiceNo { get; set; }
        public DateTime? invoiceDate { get; set; }
        public string? warehouse { get; set; }
        public string? createdBy { get; set; }
        public string? remark { get; set; }
        public string? location { get; set; }
        public List<RFIDPOList>? rfidlist { get; set; }
    }

    public class RFIDPOList
    {
        public string rfid { get; set; }
        public string poNo { get; set; }
        public int? poNoItem { get; set; }
        public string? itemCode { get; set; }
        public string? colorCode { get; set; }
        public string? size { get; set; }
        public string? uom { get; set; }
        public string? sku { get; set; }
        public string? barcode { get; set; }
        public bool? Status { get; set; }
    }
}
