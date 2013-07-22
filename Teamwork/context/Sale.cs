using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace context
{
    public class Sale
    {
        public int SaleID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Sum { get; set; }
        public int LocationID { get; set;}

        public virtual Product Product { get; set; }
        public virtual Location Location { get; set; }
    }
}
