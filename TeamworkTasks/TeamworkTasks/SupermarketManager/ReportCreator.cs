using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLStore.Data;
using Newtonsoft.Json;
using MongoDB.Bson;
using System.IO;

namespace SupermarketManager
{
    class ReportCreator
    {
        static List<ReportModel> GetData()
        {
            List<ReportModel> reportCollection = new List<ReportModel>();
            using (SQLStoreDb db = new SQLStoreDb())
            {
                var coll = (from s in db.Sales
                            select new ReportModel
                            {
                                product_id = s.ProductID,
                                product_name = s.Product.ProductName,
                                vendor_name = s.Product.Vendor.VendorName,
                                total_quantity_sold = s.Quantity,
                                total_incomes = s.Sum
                            }).GroupBy(x => x.product_id).ToList();


                foreach (var prModel in coll)
                {
                    var getFirst = prModel.First();
                    ReportModel pm = new ReportModel()
                    {
                        product_id = getFirst.product_id,
                        product_name = getFirst.product_name,
                        vendor_name = getFirst.vendor_name
                    };

                    foreach (var items in prModel)
                    {
                        pm.total_incomes += items.total_incomes;
                        pm.total_quantity_sold += items.total_quantity_sold;
                    }

                    reportCollection.Add(pm);
                }
            }

            return reportCollection;
        }

                             
        public static void RecordReports(MongoDBAccess mongodb)
        {
            string path = "Product-Reports\\";
            var reportCollection = GetData();

            
            mongodb.StoreInCollection(reportCollection);

            WriteToFileSystem(path, mongodb.GetReportObjects());
        }

        private static void WriteToFileSystem(string path, IEnumerable<ReportModel> reportCollection)
        {
            JsonSerializer serializer = new JsonSerializer();

            foreach (var rep in reportCollection)
            {
                using (StreamWriter sw = new StreamWriter(path + rep.product_id + ".json"))
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    serializer.Serialize(writer, rep);
                }
            }
        }
    }

    class ReportModel
    {
        [JsonConverter(typeof(ObjectIdConverter))]
        public ObjectId _id { get; set; }

        public int product_id { get; set; }

        public string product_name { get; set; }

        public string vendor_name { get; set; }

        public int total_quantity_sold { get; set; }

        public decimal total_incomes { get; set; }
    }

    class ObjectIdConverter : JsonConverter
    {

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value.ToString());

        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(ObjectId).IsAssignableFrom(objectType);
            //return true;
        }
    }
}

// aggregatet without signle properties
//var collection = (from s in db.Sales
//                  join prod in db.Products
//                  on s.ProductID equals prod.Id
//                  group s by s.ProductID into sgr
//                  select new ProductModel
//                  {
//                      product_id = sgr.Key,
//                      total_incomes = sgr.Sum(x => x.Sum),
//                      total_quantity_sold = sgr.Sum(x => x.Quantity)
//                  }
//                 ).ToList();

//foreach (var item in collection)
//{
//    var props = (from prod in db.Products
//                 where prod.Id == item.product_id
//                 select new
//                 {
//                     productName = prod.ProductName,
//                     vendorName = prod.Vendor.VendorName
//                 }).First();
//}

//select new ProductModel
        //                         {
        //                             product_id = prod.Id,
        //                             product_name = prod.ProductName,
        //                             vendor_name = prod.Vendor.VendorName,

        //    }