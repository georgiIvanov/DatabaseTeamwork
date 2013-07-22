using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using SQLStore.Model;
using Ionic.Zip;
using SQLStore.Data;
using System.Threading;
using System.Globalization;
using System;

namespace SupermarketManager
{
    class TransferFromExcel
    {
        static int currentDateId;

        public void ParseExcelZip(string path)
        {
            //Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            using (SQLStoreDb context = new SQLStoreDb())
            {
                using (ZipFile zip = ZipFile.Read(path))
                {
                    string currentDirectory = string.Empty;
                    foreach (var entry in zip)
                    {
                        if (!entry.IsDirectory)
                        {
                            entry.Extract("tmp", ExtractExistingFileAction.OverwriteSilently);
                            DataTable dt = ReadExcel("tmp\\" + entry.FileName);
                            ReadTable(dt, context);
                            //context.SaveChanges();
                        }
                        else
                        {
                            currentDirectory = entry.FileName.Substring(0, entry.FileName.Length - 1);
                            DateTime comparedDate = DateTime.Parse(currentDirectory);
                            var foundDate = (from date in context.SalesOnDate
                                             where date.Date == comparedDate
                                             select date).FirstOrDefault();

                            if (foundDate == null)
                            {
                                var currentDate = AddDate(currentDirectory, context);
                                context.SaveChanges();
                                currentDateId = currentDate.Id;
                            }
                            else
                            {
                                currentDateId = foundDate.Id;
                            }
                        }
                    }
                }
                
            }
        }

        private static void ReadTable(DataTable dt, SQLStoreDb context)
        {
            string locationName = dt.Rows[0].ItemArray[0].ToString();

            var foundLocation = (from loc in context.Locations
                                 where loc.LocationName == locationName
                    select loc).FirstOrDefault();

            if (foundLocation == null)
            {
                AddLocation(locationName, context);
                context.SaveChanges();
            }

            foreach (DataRow item in dt.Rows)
            {
                int productId;
                if (item.ItemArray.Length == 4 && int.TryParse(item.ItemArray[0].ToString(), out productId))
                {
                    Sale sale = new Sale();

                    sale.ProductID = productId;
                    
                    for (int i = 1; i < item.ItemArray.Length; i++)
                    {
                        if (i == 1)
                        {
                            sale.Quantity = int.Parse(item[i].ToString());
                        }
                        else if (i == 2)
                        {
                            sale.UnitPrice = decimal.Parse(item[i].ToString());
                        }
                        else if (i == 3)
                        {
                            sale.Sum = decimal.Parse(item[i].ToString());
                        }
                    }

                    sale.SaleOnDateID = currentDateId;
                    sale.LocationID = context.Locations.Where(x => x.LocationName == locationName)
                        .First().LocationID;
                    context.Sales.Add(sale);
                }
            }

            context.SaveChanges();
        }

        private static Location AddLocation(string location, SQLStoreDb context)
        {
            Location loc = new Location()
            {
                LocationName = location
            };
            context.Locations.Add(loc);

            return loc;
        }

        static SaleOnDate AddDate(string date, SQLStoreDb context)
        {
            SaleOnDate newDate = new SaleOnDate()
            {
                Date = DateTime.Parse(date)
            };

            context.SalesOnDate.Add(newDate);

            return newDate;
        }

        private static DataTable ReadExcel(string datasource)
        {
            DataTable dt = new DataTable("SupermarketSalesTable");
            OleDbConnectionStringBuilder csbuilder = new OleDbConnectionStringBuilder();
            csbuilder.Provider = "Microsoft.ACE.OLEDB.12.0";
            csbuilder.DataSource = datasource;
            csbuilder.Add("Extended Properties", "Excel 12.0 Xml;HDR=YES");

            OleDbConnection connection = new OleDbConnection(csbuilder.ConnectionString);
            connection.Open();
            using (connection)
            {
                string selectSql = @"SELECT * FROM [Sales$]";
                using (OleDbDataAdapter adapter = new OleDbDataAdapter(selectSql, connection))
                {
                    adapter.FillSchema(dt, SchemaType.Source);
                    adapter.Fill(dt);
                }
            }

            return dt;
        }
    }
}
