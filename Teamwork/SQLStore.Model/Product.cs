using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLStore.Model
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public decimal BasePrice { get; set; }

        public int MeasureId { get; set; }
        [ForeignKey("MeasureId")]
        public virtual Measure Measure { get; set; }

        public string ProductName { get; set; }


        public int VendorId { get; set; }

        [ForeignKey("VendorId")]
        public virtual Vendor Vendor { get; set; }
    }
}
