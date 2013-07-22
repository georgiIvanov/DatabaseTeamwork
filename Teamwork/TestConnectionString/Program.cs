using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using context;
using Ionic.Zip;
using System.IO;
using System.Data.OleDb;
using System.Data;

namespace TestConnectionString
{
    class Program
    {
        static void Main(string[] args)
        {
            SupermarketSalesContext context = new SupermarketSalesContext();

            PDFFileCreator.CreateTable("fu");

            //using (var sw = new StreamWriter("temp.out"))
            //{
            //    using (ZipFile zip = ZipFile.Read("..\\..\\Sample-Sales-Reports.zip"))
            //    {
            //        string currentDirectory = string.Empty;
            //        foreach (var entry in zip)
            //        {
            //            if (!entry.IsDirectory)
            //            {
            //                entry.Extract("tmp", ExtractExistingFileAction.OverwriteSilently);
            //                DataTable dt = ReadExcel("tmp\\" + entry.FileName);
            //                ReadTable(dt, context);
            //            }
            //            else
            //            {

            //                currentDirectory = entry.FileName;
            //            }

            //            context.SaveChanges();
            //        }
            //    }
            //}
        }

        private static void ReadTable(DataTable dt, SupermarketSalesContext context)
        {
            Location location = new Location();
            location.LocationName = dt.Rows[0].ItemArray[0].ToString();
            context.Locations.Add(location);
            context.SaveChanges();

            foreach (DataRow item in dt.Rows)
            {
                int num;
                if (item.ItemArray.Length == 4 && int.TryParse(item.ItemArray[0].ToString(), out num))
                {
                    Sale sale = new Sale();

                    for (int i = 0; i < item.ItemArray.Length; i++)
                    {
                        if (i == 0)
                        {
                            sale.ProductID = int.Parse(item[i].ToString());
                        }
                        else if (i == 1)
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

                    sale.LocationID = context.Locations.Where(x => x.LocationName == location.LocationName)
                        .First().LocationID;
                    context.Sales.Add(sale);
                }
            }
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
