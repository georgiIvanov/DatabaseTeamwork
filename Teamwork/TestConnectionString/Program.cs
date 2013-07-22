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
            //TestContext db = new TestContext();

            //db.Persons.Add(new Person() { Age = 18 });
            //db.SaveChanges();

            //foreach (var item in db.Persons)
            //{
            //    Console.WriteLine(item.Age);
            //}

            using (var sw = new StreamWriter("temp.out"))
            {
                using (ZipFile zip = ZipFile.Read("..\\..\\Sample-Sales-Reports.zip"))
                {
                    string currentDirectory = string.Empty;
                    foreach (var entry in zip)
                    {
                        if (!entry.IsDirectory)
                        {
                            entry.Extract("tmp", ExtractExistingFileAction.OverwriteSilently);
                            DataTable dt = ReadExcel("tmp\\" + entry.FileName);

                            foreach (DataRow item in dt.Rows)
                            {
                                foreach (DataColumn column in dt.Columns)
                                {
                                    if (!string.IsNullOrWhiteSpace(item[column].ToString()))
                                    {
                                        Console.WriteLine(item[column]);
                                    }

                                }
                            }
                        }
                        else
                        {
                            currentDirectory = entry.FileName;
                        }
                    }
                }
            }
        }

        private static DataTable ReadExcel(string datasource)
        {
            DataTable dt = new DataTable("newtable");
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
