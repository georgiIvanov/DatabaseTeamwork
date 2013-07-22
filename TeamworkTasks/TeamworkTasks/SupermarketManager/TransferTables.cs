using MySQLStore;
using SQLStore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupermarketManager
{
    class TransferTables
    {
        public void TransferFromMySqlToSQLServer()
        {
            using (var mySqlStore = new StoreModel())
            {
                using (var sqlStore = new SQLStoreDb())
                {
                    //DeleteTables(sqlStore);

                    foreach (var mySqlVendor in mySqlStore.Vendors)
                    {
                        sqlStore.Vendors.Add(new SQLStore.Model.Vendor()
                        {
                            Id = mySqlVendor.ID,
                            VendorName = mySqlVendor.VendorName
                        });
                    }

                    foreach (var mySqlMeasure in mySqlStore.Measures)
                    {
                        sqlStore.Measures.Add(new SQLStore.Model.Measure()
                        {
                            Id = mySqlMeasure.ID,
                            MeasureName = mySqlMeasure.MeasuresName
                        });
                    }

                    foreach (var mySqlProduct in mySqlStore.Products)
                    {
                        sqlStore.Products.Add(new SQLStore.Model.Product()
                        {
                            Id = mySqlProduct.ID,
                            ProductName = mySqlProduct.ProductName,
                            BasePrice = mySqlProduct.BasePrice,
                            MeasureId = mySqlProduct.MeasureID,
                            VendorId = mySqlProduct.VendorID
                        });
                    }

                    



                    sqlStore.SaveChanges();



                }
            }

        }

        private static void DeleteTables(SQLStoreDb sqlStore)
        {
            foreach (var prod in sqlStore.Products)
            {
                sqlStore.Products.Remove(prod);
            }

            foreach (var measure in sqlStore.Measures)
            {
                sqlStore.Measures.Remove(measure);
            }

            foreach (var vend in sqlStore.Vendors)
            {
                sqlStore.Vendors.Remove(vend);
            }

            sqlStore.SaveChanges();
        }
    }
}
