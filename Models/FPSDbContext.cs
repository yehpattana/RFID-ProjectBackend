using Microsoft.EntityFrameworkCore;
using RFIDApi.DTO.Data;
using RFIDApi.Models.FPS;

namespace RFIDApi.Models
{
    public class FPSDbContext : DbContext
    {
        public FPSDbContext(DbContextOptions<FPSDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<MasterProductOnline>().HasKey(p => new { p.CompanyCode, p.ItemCode, p.ColorCode, p.Size });
            modelBuilder.Entity<WarehouseInOutType>().HasKey(p => new { p.InoutType, p.TranType });
            modelBuilder.Entity<Purchase_PODetail>().HasKey(p => new { p.PRNo, p.ItemNo,p.DlvCode,p.CustomerPO,p.Size });
            modelBuilder.Entity<FPSWarehouseTransection>().HasKey(p => new { p.ReceiveNo, p.RFId });

            modelBuilder.Entity<MasterProductOnline>().HasMany(p => p.WarehouseRFIDs).WithOne(w => w.MasterProductOnline).HasForeignKey(f => new { f.ItemCode,f.Size,f.ColorCode,f.CompanyCode});
            // Entities relation with transaction
            modelBuilder.Entity<WarehouseRFID>().HasMany(p => p.WarehouseTransections).WithOne(w => w.WarehouseRFID).HasForeignKey(f => f.RFId);
            modelBuilder.Entity<WarehouseReceive>().HasMany(p => p.WarehouseTransections).WithOne(w => w.WarehouseReceive).HasForeignKey(f => f.ReceiveNo);

            modelBuilder.Entity<GenReceiveNoResultDTO>().HasNoKey().ToView(null); // บอก EF ว่าไม่ผูก table จริง

            base.OnModelCreating(modelBuilder);
        }
        public DbSet<MasterProductOnline> masterProductOnlines { get; set; }
        public DbSet<MasterWarehouse> masterWarehouses { get; set; }
        public DbSet<WarehouseInOutType> warehouseInOutTypes
        {
            get; set;
        }
        public DbSet<FPSWarehouseTransection> warehouseTransections { get; set; }
        public DbSet<WarehouseRFID> warehouseRFIDs
        {
            get; set;
        }
        public DbSet<WarehouseReceive> warehouseReceives
        {
            get; set;
        }
        public DbSet<Purchase_PODetail> purchase_PODetails
        {
            get; set;
        }
        public DbSet<Purchase_PODesc> purchase_PODescs
        {
            get; set;
        }
    }
}
