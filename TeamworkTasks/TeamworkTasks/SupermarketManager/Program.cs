using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySQLStore;
using SQLStore.Data;
using SQLStore.Model;
using System.Data.Entity;

namespace SupermarketManager
{
    class Program
    {
        static void Main(string[] args)
        {


            TransferTables transferTables = new TransferTables();
            transferTables.TransferFromMySqlToSQLServer();

            TransferFromExcel transferExcel = new TransferFromExcel();
            transferExcel.ParseExcelZip("zip\\Sample-Sales-Reports.zip");


            GeneratePDF.CreateTable("test.pdf");

            XMLCreator.CreateXml("Sales-by-Vendors-report.xml");

            string dbName = "productreports";
            string collectionName = "reports";
            string expensesCollectionName = "expenses";
            MongoDBAccess mongodb = new MongoDBAccess(dbName, collectionName, expensesCollectionName);

            ReportCreator.RecordReports(mongodb);
            
            XMLReader.ReadXml("Vendors-Expenses.xml");

            ReportCreator.RecordExpenses(mongodb);

            using (SQLStoreDb db = new SQLStoreDb())
            {
                var a = from w in db.SalesOnDate
                        select w;

                //var r = from tt in db.Sales
                //        select tt;
            }

        }
    }
}
