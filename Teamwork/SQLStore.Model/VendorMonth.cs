using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SQLStore.Model
{
    public class VendorMonth
    {
        public int VendorID { get; set; }
        public int MonthID { get; set; }
        public decimal Expenses { get; set; }

        //[ForeignKey("VendorID")]
        public virtual Vendor Vendor { get; set; }
        //[ForeignKey("MonthID")]
        public virtual Month Month { get; set; }
    }
}
