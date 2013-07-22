using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySQLStore;

namespace SupermarketManager
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var store = new StoreModel())
            {
                foreach (var item in store.Vendors)
                {
                    Console.WriteLine(item.VendorName);
                }
            }
        }
    }
}
