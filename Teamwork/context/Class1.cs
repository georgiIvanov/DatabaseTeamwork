using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace context
{
    public class TestContext : DbContext
    {
        public TestContext()
            :base("TestDb")
        {
        }
        public DbSet<Person> Persons { get; set; }
    }

    public class Person
    {
        public int Id { get; set; }
        public int Age { get; set;}
    }
}
