using Microsoft.EntityFrameworkCore;
using RFIDApi.Models.System;

namespace RFIDApi.Models.Context
{
    public class SystemDBContext : DbContext
    {

        public SystemDBContext(DbContextOptions<SystemDBContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            base.OnModelCreating(modelBuilder);
        }


        public DbSet<SYS_Company> _Companies { get; set; }
    }
}
