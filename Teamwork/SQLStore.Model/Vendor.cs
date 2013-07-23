using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLStore.Model
{
    public class Vendor
    {
        private ICollection<VendorMonth> vendorMonths;

        public Vendor()
        {
            this.vendorMonths = new HashSet<VendorMonth>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public string VendorName { get; set; }

        public virtual ICollection<VendorMonth> VendorMonths
        {
            get
            {
                return this.vendorMonths;
            }
            set
            {
                this.vendorMonths = value;
            }
        }
    }
}
