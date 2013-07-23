using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLiteStore;

namespace SupermarketManager
{
    public static  class SQLitePrint
    {
        public static void Print()
        {
            using(var sqlLiteContext = new SQLiteDatabaseEntities())
            {
                foreach (var item in sqlLiteContext.ProductTaxes)
                {
                    Console.WriteLine(item.ProductName + " " + item.Tax);
                }
            }
        }
    }
}
