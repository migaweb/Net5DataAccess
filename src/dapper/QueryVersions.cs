using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using code.ado.shared;
using Dapper;
using System.Data;
using System.Data.SQLite;

namespace dapper
{
    internal class QueryVersions
    {
        public static void Run()
        {
            // ReadFirst();
            // ReadSingle();
            // EasyJoin();
            Paging();
        }

        private static IDbConnection Connect => new SQLiteConnection(Constants.SQLITE_CONNECTION);

        private static void ReadFirst()
        {
            string first = "SELECT o.Name AS OrderName, o.CustomerId as Fk, o.Id as OrderIndex FROM orders AS o";
            using (var connection = Connect)
            {
                connection.Open();
                var customer = connection.QueryFirst<TransformedOrder>(first);
                System.Console.WriteLine(customer);
            }
        }

        private static void ReadSingle()
        {
            string sql = "SELECT * FROM customers WHERE id > 9000";
            using (var connection = Connect)
            {
                connection.Open();
                try
                {
                    var result = connection.QuerySingle<CustomerV2>(sql);
                    System.Console.WriteLine(result);
                }
                catch (System.Exception ex)
                {
                    System.Console.WriteLine("single threw! " + ex.Message);
                }
            }
        }

        private static void EasyJoin()
        {
            var sql = "SELECT * FROM orders AS o INNER JOIN customers AS c on c.Id = o.CustomerId";
            using (var connect = Connect)
            {
                connect.Open();
                var result = connect.Query<OrderV2, CustomerV2, OrderV2>(sql,
                    (o2, c2) => 
                    {
                        o2.Customer = c2;
                        return o2;
                    }
                );
                System.Console.WriteLine(string.Join(",", result));
            }
        }

        private static void Paging()
        {
            string limit = "SELECT * FROM orders LIMIT 2;SELECT COUNT(*) FROM orders";
            using (var connection = Connect.QueryMultiple(limit))
            {
                var orders = connection.Read<OrderV2>();
                var all = connection.Read<int>();
                System.Console.WriteLine("returned : " + orders.Count() + $" and COUNT(*) is {all.First()}");
            }
        }


        private class TransformedOrder
        {
            public string OrderName { get; set; } // Name
            public int Fk { get; set; } // Customer id
            public int Index { get; set; } // id
            public override string ToString()
                => $"[OrderName: {OrderName}, Fk :{Fk}]";
        }

        private class OrderV2
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public CustomerV2 Customer { get; set; }

            public override string ToString()
                => $"[o.id={Id}, o.Name={Name}, c.Name={Customer.Name}+{Customer.LastName}]";
        }

        private class CustomerV2
        {
            public int Id { get; set; }
            public string LastName { get; set; }
            public string Name { get; set; }
        }
    }
}