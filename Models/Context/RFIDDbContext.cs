using Microsoft.EntityFrameworkCore;

namespace RFIDApi.Models.Context // เปลี่ยน namespace ตามโปรเจกต์ใหม่
{
    public class RFIDDbContext : DbContext
    {
        public RFIDDbContext(DbContextOptions<RFIDDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<ProductRFID>().HasKey(p => new { p.SKU, p.RFID });

            modelBuilder.Entity<ProductRFID>()
            .HasOne(pr => pr.Product)
            .WithMany(p => p.productRFIDs)
            .HasForeignKey(pr => pr.SKU);

            base.OnModelCreating(modelBuilder);
        }


        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductRFID> ProductsRFID { get; set; }
        public DbSet<ShopifySalesDaily> shopifySalesDailies { get; set; }
        public DbSet<ShopifyWarehouseStock> shopifyWarehouseStocks { get; set; }

    }
}