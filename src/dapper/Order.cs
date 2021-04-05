using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace dapper
{
    internal class Order
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CustomerId { get; set; }
    }
}