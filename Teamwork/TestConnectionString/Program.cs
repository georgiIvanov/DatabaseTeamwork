using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using context;

namespace TestConnectionString
{
    class Program
    {
        static void Main(string[] args)
        {
            TestContext db = new TestContext();

            db.Persons.Add(new Person() { Age = 18 });
            db.SaveChanges();

            foreach (var item in db.Persons)
            {
                Console.WriteLine(item.Age);
            }
        }
    }
}
