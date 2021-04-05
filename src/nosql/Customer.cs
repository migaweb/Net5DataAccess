using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace nosql
{
    internal class Customer : IFormattable
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public List<Order> Orders { get; set; }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (format == "redis")
                return JsonConvert.SerializeObject(this);
            else if (format == "output")
                return $"Customer: {Name} {LastName} with Orders: '{string.Join(",", this.Orders)}";
            else 
                return ToString();
        }
    }
}