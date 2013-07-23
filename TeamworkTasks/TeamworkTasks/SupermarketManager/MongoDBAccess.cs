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
    class MongoDBAccess
    {
        MongoClient client;
        MongoServer server;
        MongoDatabase dictionaryDB;
        MongoCollection<ReportModel> collection;
        IQueryable<ReportModel> queryableCollection;

        public MongoDBAccess(string database, string collection)
        {
            this.client = new MongoClient("mongodb://localhost");
            this.server = client.GetServer();
            this.dictionaryDB = server.GetDatabase(database);

            this.collection = dictionaryDB.GetCollection<ReportModel>(collection);
            //queryableCollection = this.collection.AsQueryable<ReportModel>();
        }

        public void StoreInCollection(ReportModel obj)
        {
            collection.Insert(obj);
        }

        public void StoreInCollection(List<ReportModel> obj)
        {
            foreach (var item in obj)
            {
                collection.Insert(item);
            }
        }

        public ReportModel GetFromCollecion(int key)
        {
            var obj = queryableCollection.Where(x => x.product_id == key).First();

            return obj;
        }

        public IEnumerable<ReportModel> GetReportObjects()
        {
            return collection.FindAll();
        }
    }
}
