using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLStore.Model
{
    public class Month
    {
        private ICollection<VendorMonth> vendors;

        public Month()
        {
            this.vendors = new HashSet<VendorMonth>();
        }

        public int MonthId { get; set; }
        public DateTime MonthDate { get; set; }

        public ICollection<VendorMonth> Vendors
        {
            get
            {
                return this.vendors;
            }
            set
            {
                this.vendors = value;
            }
        }
    }
}
