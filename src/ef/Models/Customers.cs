using System;
using System.Collections.Generic;

namespace code.ef.Models
{
    public partial class Customers
    {
        public Customers()
        {
            Orders = new HashSet<Orders>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }

        public virtual ICollection<Orders> Orders { get; set; }

        public override string ToString()
            => $"Customer: {Lastname}, {Name}";
    }
}
