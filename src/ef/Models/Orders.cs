using System;
using System.Collections.Generic;

namespace code.ef.Models
{
    public partial class Orders
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? Customerid { get; set; }

        public virtual Customers Customer { get; set; }
    }
}
