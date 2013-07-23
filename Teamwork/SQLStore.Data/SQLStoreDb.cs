using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLStore.Model;

namespace SQLStore.Data
{
    public class SQLStoreDb : DbContext
    {
        public SQLStoreDb()
            : base("SupermarketDb")
        {

        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Measure> Measures { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<SaleOnDate> SalesOnDate { get; set; }
        public DbSet<Month> Months { get; set; }
        public DbSet<VendorMonth> VendorMonths { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<VendorMonth>().HasKey(vm => new { vm.VendorID, vm.MonthID });
            base.OnModelCreating(modelBuilder);
        }
    }
}
