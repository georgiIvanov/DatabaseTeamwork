using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLStore.Model
{
    public class Product
    {
        public int Id { get; set; }
        public decimal BestPrice { get; set; }
        public int MeasureId { get; set; }
        public string ProductName { get; set; }
        public int VendorId { get; set; }
    }
}
