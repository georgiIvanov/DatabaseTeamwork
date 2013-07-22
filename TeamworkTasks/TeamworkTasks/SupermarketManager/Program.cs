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

        }
    }
}
