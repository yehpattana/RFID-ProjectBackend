using Microsoft.EntityFrameworkCore; // แทน System.Data.Entity

namespace RFIDApi.Models // เปลี่ยน namespace ตามโปรเจกต์ใหม่
{
    public class RFIDDbContext : DbContext
    {
        public RFIDDbContext(DbContextOptions<RFIDDbContext> options) : base(options)
        {
        }

        public DbSet<RFIDTag> RFIDTags { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<PrinterReceipt> StockTransactions { get; set; }
        public DbSet<Sales> Sales { get; set; }
        public DbSet<SalesDetail> SalesDetails { get; set; }
        public DbSet<Customer> Customers { get; set; }


        // NDS

        public DbSet<POS_NDS_Barcode> POS_NDS_Barcodes { get; set; }
        public DbSet<POS_NDS_Brand> POS_NDS_Brands { get; set; }
        public DbSet<POS_NDS_Category> POS_NDS_Categorys { get; set; }
        public DbSet<POS_NDS_ColorCode> POS_NDS_ColorCodes { get; set; }
        public DbSet<POS_NDS_Image> POS_NDS_Images { get; set; }
        public DbSet<POS_NDS_MasterShop> POS_NDS_MasterShops { get; set; }
        public DbSet<POS_NDS_MasterStock> POS_NDS_MasterStocks { get; set; } 
        public DbSet<POS_NDS_Order> POS_NDS_Orders { get; set; }
        public DbSet<POS_NDS_OrderDetail> POS_NDS_OrderDetails { get; set; }
        public DbSet<POS_NDS_PaymentMethod> POS_NDS_PaymentMethods { get; set; }
        public DbSet<POS_NDS_Permission> POS_NDS_Permissions { get; set; }
        public DbSet<POS_NDS_Product> POS_NDS_Products { get; set; } 
        public DbSet<POS_NDS_RFID> POS_NDS_RFIDs { get; set; }
        public DbSet<POS_NDS_Role> POS_NDS_Roles { get; set; }
        public DbSet<POS_NDS_RolePermission> POS_NDS_RolePermissions { get; set; }
        public DbSet<POS_NDS_Size> POS_NDS_Sizes { get; set; } 
        public DbSet<POS_NDS_StatusTransaction> POS_NDS_StatusTransactions { get; set; }
        public DbSet<POS_NDS_StockInShop> POS_NDS_StockInShops { get; set; }
        public DbSet<POS_NDS_StockTransaction> POS_NDS_StockTransactions { get; set; }
        public DbSet<POS_NDS_Style> POS_NDS_Styles { get; set; } 
        public DbSet<POS_NDS_TransactionType> POS_NDS_TransactionTypes { get; set; } 
        public DbSet<POS_NDS_Unit> POS_NDS_Units { get; set; }
        public DbSet<POS_NDS_User> POS_NDS_Users { get; set; }
        public DbSet<POS_NDS_UserShop> POS_NDS_UserShops { get; set; }
        public DbSet<POS_NDS_Variant> POS_NDS_Variants { get; set; }   
        public DbSet<POS_NDS_VariantStyle> POS_NDS_VariantStyles { get; set; }
        public DbSet<POS_NDS_Warehouse> POS_NDS_Warehouses { get; set; }
        public DbSet<POS_NDS_WarehouseStock> POS_NDS_WarehouseStocks { get; set; }
        public DbSet<POS_NDS_Warehouse_Shelf> POS_NDS_Warehouse_Shelfs { get; set; }

    }
}