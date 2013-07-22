using SQLStore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupermarketManager
{
    class GeneratePDF
    {
        public GeneratePDF()
        {

        }

        public void CreatePDF()
        {
            using (SQLStoreDb db = new SQLStoreDb())
            {
                var collection = (from sale in db.Sales
                                  join loc in db.Locations
                                  on sale.LocationID equals loc.LocationID
                                  select new
                                  {
                                      ProductName = sale.Product.ProductName,
                                      Sum = sale.Sum,
                                      Quantity = sale.Quantity,
                                      UnitPrice = sale.UnitPrice,
                                      Location = sale.Location.LocationName,
                                      Date = sale.SaleOnDate.Date
                                  }).GroupBy(x => x.Date).ToList();
                                 


            }
        }
    }
}
