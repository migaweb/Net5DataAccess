using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace nosql
{
    internal class Order
    {
        public string Text { get; set; }
        public int Costs { get; set; }

        public override string ToString()
        {
            return Text;
        }
    }
}