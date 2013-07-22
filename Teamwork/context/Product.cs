using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace context
{
    public class Product
    {
        public int ProductID { get; set; }
        public int VendorID { get; set; }
        public string ProductName { get; set; }
        public int MeasureID { get; set; }
        public decimal BasePrice { get; set; }

        public virtual Vendor Vendor { get; set; }
        public virtual Measure Measure { get; set; }
    }
}
