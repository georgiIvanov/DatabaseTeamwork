using SQLStore.Data;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLiteStore;

namespace SupermarketManager
{
    //TransferToExcel.WriteToExcel("Products-Total-Report.xlsx", mongodb);
    public static class TransferToExcel
    {
        public static void WriteToExcel(string fileName, MongoDBAccess mongodb)
        {
            var reports = mongodb.GetReportObjects();
            var expenses = mongodb.GetExpenseObjects();

            string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Password="""";
User ID=Admin;Data Source=..\..\Products-Total-Report.xls;Extended Properties=""HDR=YES;"";
Jet OLEDB:Engine Type=37";

            OleDbConnection dbConn = new OleDbConnection(connectionString);

            dbConn.Open();
            using (var context = new SQLStoreDb())
            {
                using (var sqlLiteContext = new SQLiteDatabaseEntities())
                {
                    using (dbConn)
                    {
                        OleDbCommand command = new OleDbCommand("CREATE TABLE Result ([Vendor] char(255), [Incomes] decimal, [Expenses] decimal, [Taxes] decimal, [Financial Result] decimal)");
                        command.Connection = dbConn;
                        command.ExecuteNonQuery();

                        foreach (var vendor in context.Vendors)
                        {
                            var currentVendorProducts = reports.Where(x => x.vendor_name == vendor.VendorName);
                            var incomes = currentVendorProducts.Sum(x => x.total_incomes);
                            var exps = expenses.Where(x => x.vendor_name == vendor.VendorName && x.month.Month == DateTime.Now.Month).FirstOrDefault();
                            var taxes = from r in currentVendorProducts
                                        join t in sqlLiteContext.ProductTaxes on r.product_name equals t.ProductName
                                        select new
                                        {
                                            Tax = t.Tax
                                        };
                            decimal? tax = taxes.Sum(x => x.Tax) / 100m;
                            //var taxes = sqlLiteContext.ProductTaxes.Where(x => currentVendorProducts.All(y => y.product_name == x.product_name));
                            if (exps != null)
                            {

                                OleDbCommand cmd = new OleDbCommand(
                                    "INSERT INTO [Result$]([Vendor], [Incomes], [Expenses], [Taxes], [Financial Result]) VALUES(@vendor, @incomes, @expenses, @taxes, @result)", dbConn);
                                cmd.Parameters.AddWithValue("@vendor", vendor.VendorName);
                                cmd.Parameters.AddWithValue("@incomes", incomes);
                                cmd.Parameters.AddWithValue("@expenses", exps.expenses);
                                cmd.Parameters.AddWithValue("@taxes", tax);
                                cmd.Parameters.AddWithValue("@result", (incomes - exps.expenses - tax));

                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                }
            }
        }
    }
}
