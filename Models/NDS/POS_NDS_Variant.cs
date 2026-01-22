
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace RFIDApi.Models
{
    [Table("POS_NDS_Variant")]
    public class POS_NDS_Variant
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VariantId { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }

        [ForeignKey("Size")]
        public int SizeId { get; set; }

        [ForeignKey("ColorCode")]
        public int ColorId { get; set; }

        [ForeignKey("Unit")]
        public int UnitId { get; set; }

        [Required]
        public string SKU { get; set; }

        public decimal Price { get; set; }

        public int Status { get; set; }

        public string? Remark { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public string? CreateBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string UpdateBy { get; set; } = "Not Updated";

        public DateTime? Delete_At { get; set; }

        // Navigation Properties
        public virtual POS_NDS_Product? Product { get; set; }
        public virtual POS_NDS_Size? Size { get; set; }
        public virtual POS_NDS_ColorCode? ColorCode { get; set; }
        public virtual POS_NDS_Unit? Unit { get; set; }
        public virtual ICollection<POS_NDS_Image>? Images { get; set; }
        public virtual ICollection<POS_NDS_RFID>? RFIDs { get; set; }
        public virtual ICollection<POS_NDS_Barcode>? Barcodes { get; set; }
        public virtual ICollection<POS_NDS_MasterStock>? MasterStocks { get; set; }
        public virtual ICollection<POS_NDS_WarehouseStock>? WarehouseStocks { get; set; }
        public virtual ICollection<POS_NDS_StockInShop>? StockInShops { get; set; }
        public virtual ICollection<POS_NDS_StockTransaction>? StockTransactions { get; set; }
        public virtual ICollection<POS_NDS_OrderDetail>? OrderDetails { get; set; }
        public virtual ICollection<POS_NDS_VariantStyle>? VariantStyles { get; set; }
    }
}