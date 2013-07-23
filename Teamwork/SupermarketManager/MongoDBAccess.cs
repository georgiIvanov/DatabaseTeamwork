using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB;
using MongoDB.Bson;
using MongoDB.Driver;

namespace SupermarketManager
{
    public class MongoDBAccess
    {
        MongoClient client;
        MongoServer server;
        MongoDatabase dictionaryDB;
        MongoCollection<ReportModel> reportCollection;
        MongoCollection<ExpenseModel> expensesCollection;

        IQueryable<ReportModel> queryableCollection;

        public MongoDBAccess(string database, string reportCollection, string expensesCollection)
        {
            this.client = new MongoClient("mongodb://localhost");
            this.server = client.GetServer();
            this.dictionaryDB = server.GetDatabase(database);

            this.reportCollection = dictionaryDB.GetCollection<ReportModel>(reportCollection);
            this.expensesCollection = dictionaryDB.GetCollection<ExpenseModel>(expensesCollection);
            //queryableCollection = this.collection.AsQueryable<ReportModel>();
        }

        public void StoreInCollection(ReportModel obj)
        {
            reportCollection.Insert(obj);
        }

        public void StoreInReportCollection(List<ReportModel> obj)
        {
            foreach (var item in obj)
            {
                reportCollection.Insert(item);
            }
        }

        public void StoreInExpensesCollection(List<ExpenseModel> obj)
        {
            foreach (var item in obj)
            {
                expensesCollection.Insert(item);
            }
        }

        public ReportModel GetFromCollecion(int key)
        {
            var obj = queryableCollection.Where(x => x.product_id == key).First();

            return obj;
        }

        public IEnumerable<ReportModel> GetReportObjects()
        {
            return reportCollection.FindAll();
        }

        public IEnumerable<ExpenseModel> GetExpenseObjects()
        {
            return expensesCollection.FindAll();
        }

    }
}
