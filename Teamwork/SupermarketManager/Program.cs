using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySQLStore;
using SQLStore.Data;
using SQLStore.Model;
using System.Data.Entity;
using SQLiteStore;

namespace SupermarketManager
{
    class Program
    {
        static void Main(string[] args)
        {


            //TransferTables transferTables = new TransferTables();
            //transferTables.TransferFromMySqlToSQLServer();

            //TransferFromExcel transferExcel = new TransferFromExcel();
            //transferExcel.ParseExcelZip("zip\\Sample-Sales-Reports.zip");


            //GeneratePDF.CreateTable("test.pdf");

            //XMLCreator.CreateXml("Sales-by-Vendors-report.xml");

            string dbName = "productreports";
            string reportCollectionName = "reports";
            string expensesCollectionName = "expenses";
            MongoDBAccess mongodb = new MongoDBAccess(dbName, reportCollectionName, expensesCollectionName);

            ReportCreator.RecordReports(mongodb);
            ReportCreator.RecordExpenses(mongodb);


            //XMLReader.ReadXml("Vendors-Expenses.xml");

            TransferToExcel.WriteToExcel("Products-Total-Report.xlsx", mongodb);
        }
    }
}
