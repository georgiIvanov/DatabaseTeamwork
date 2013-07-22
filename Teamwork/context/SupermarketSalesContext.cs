using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace context
{
    public class SupermarketSalesContext : DbContext
    {
        public SupermarketSalesContext()
            : base("SupermarketDB")
        {
        }

        public DbSet<Measure> Measures { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<Location> Locations { get; set; }
    }
}
