using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace dapper
{
    internal class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }

        public List<Order> Orders { get; set; }

        public override string ToString()
            => $"Customer: {Name} {LastName}";
    }
}