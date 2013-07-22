using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySQLStore;
using SQLStore.Data;
using SQLStore.Model;

namespace SupermarketManager
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var store = new StoreModel())
            {
                foreach (var item in store.Products)
                {
                    Console.WriteLine(item.ProductName);
                }
            }

            using (var sqlStore = new SQLStoreDb())
            {
                var aa = from a in sqlStore.Products
                         select a;

            }
        }
    }
}
