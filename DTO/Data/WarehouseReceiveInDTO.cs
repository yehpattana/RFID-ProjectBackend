namespace RFIDApi.DTO.Data
{
    public class WarehouseReceiveInDTO
    {
        public string? receiveNo { get; set; }
        public DateTime? receiveDate { get; set; }
        public string? receiveType { get; set; }
        public string? companyCode { get; set; }
        public string? deliveryNo { get; set; }
        public string? invoiceNo { get; set; }
        public DateTime? invoiceDate { get; set; }
        public string? warehouse { get; set; }
        public string? createdBy { get; set; }
        public string? remark { get; set; }
        public List<RFIDPOList>? rfidlist { get; set; }
    }
}
