using Microsoft.EntityFrameworkCore; // แทน System.Data.Entity

namespace RFIDApi.Models // เปลี่ยน namespace ตามโปรเจกต์ใหม่
{
    public class RFIDDbContext : DbContext
    {
        public RFIDDbContext(DbContextOptions<RFIDDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ProductRFID>().HasKey(p => new { p.SKU, p.RFID });

            modelBuilder.Entity<ProductRFID>()
            .HasOne(pr => pr.Product)
            .WithMany(p => p.productRFIDs)
            .HasForeignKey(pr => pr.SKU);
        }


        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductRFID> ProductsRFID { get; set; }
        public DbSet<ShopifySalesDaily> shopifySalesDailies { get; set; }
        public DbSet<ShopifyWarehouseStock> shopifyWarehouseStocks { get; set; }

    }
}