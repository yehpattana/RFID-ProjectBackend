using RFIDApi.Models.FPS;

namespace RFIDApi.DTO.Data
{
    public class WarehouseOutstockDTO
    {
        public RequestOutHeaderDto Header { get; set; } = new();
        public List<RequestOutItemDto> Items { get; set; } = new();
    }
    public class WarehouseTransactionCheckOutRequest
    {
        public string ItemCode { get; set; } = string.Empty;
        public string ColorCode { get; set; } = string.Empty;
        public string Size { get; set; } = string.Empty;
        public string UOM { get; set; }
        public int BalanceQty { get; set; }
    }

    public class WarehouseRequestOutMergeDTO
    {
        // ===== Main =====
        public string OutNo { get; set; } = null!;
        public DateTime RequestDate { get; set; }
        public string RequestBy { get; set; } = null!;
        public string OutType { get; set; } = null!;
        public string CreateBy { get; set; } = null!;
        public DateTime CreateDate { get; set; }
        public string EditBy { get; set; } = null!;
        public DateTime EditDate { get; set; }

        // ===== Detail (LEFT JOIN) =====
        public string? ItemCode { get; set; }
        public string? ColorCode { get; set; }
        public string? Size { get; set; }
        public int? OutQty { get; set; }
        public string? UOM { get; set; }
        public bool? OutStatus { get; set; }
    }
    public class RequestOutHeaderDto
    {
        public string RequestNo { get; set; } = string.Empty; // auto / generate
        public DateTime RequestDate { get; set; }
        public string OutType { get; set; } = string.Empty;   // from Warehouse_InoutType
        public string RequestBy { get; set; } = string.Empty; // user login
        public string? OutsourcePONo { get; set; }             // optional (POType = 3)
        public string? CreateBy { get; set; }
    }

    public class RequestOutItemDto
    {
        public string ProductCode { get; set; } = string.Empty;
        public string? Color { get; set; }
        public string? Size { get; set; }
        public int Qty { get; set; }
        public string Uom { get; set; } = string.Empty;
    }

    public class OutstockRequestDto
    {
        public RequestOutHeaderDto Header { get; set; } = new();
        public List<RequestOutItemDto> Items { get; set; } = new();
    }

    public class ScanOutStockRequestDto
    {
        public string RequestOutNo { get; set; } = string.Empty;
        public string OutType { get; set; } = string.Empty;
        public DateTime OutDate { get; set; }

        public string ProductCode { get; set; } = string.Empty;
        public string? Color { get; set; }
        public string? Size { get; set; }

        public decimal OutQty { get; set; }
    }
}
