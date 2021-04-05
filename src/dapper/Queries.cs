using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using code.ado.shared;
using Dapper;

namespace dapper
{
    internal class Queries
    {
        // private static string joinSql = @"
        //     SELECT 
        //         c.*,
        //         o.*
        //     FROM Customers c
        //     INNER JOIN Orders o
        //     ON o.CustomerId = c.id
        //     ";

        public static void Run()
        {
            // Read();
            // ReadStrong();
            // Join();
        }

        private static void Read()
        {
            using (var connection = new Npgsql.NpgsqlConnection(Constants.PSQL_CONNECTION))
            {
                var customers = connection.Query(sql).FirstOrDefault();
                System.Console.WriteLine(customers);
            }
        }
        private static string sql = "SELECT * FROM Customers";
        private static void ReadStrong()
        {
            using (var connection = new Npgsql.NpgsqlConnection(Constants.PSQL_CONNECTION))
            {
                var customers = connection.Query<Customer>(sql).ToList();
                System.Console.WriteLine(string.Join(Environment.NewLine, customers));
            }
        }


        private static string joinSql = @"
            SELECT 
            c.*,
            o.*
            FROM Customers c
            INNER JOIN Orders o
            ON o.CustomerId = c.id
        ";
        private static void Join()
        {
            List<Customer> customers;
            var map = new Dictionary<int, Customer>();
            using (var connection = new Npgsql.NpgsqlConnection(Constants.PSQL_CONNECTION))
            {
                customers = connection.Query<Customer, Order, Customer>(
                    joinSql,
                    (c, o) => 
                    {
                        Customer entity = c;
                        if (map.ContainsKey(c.Id))
                        {
                            entity = map[c.Id];
                        }
                        else
                        {
                            map.Add(c.Id, entity);
                            entity.Orders = new List<Order>();
                        }
                        entity.Orders.Add(o);
                        return entity;
                    }
                    , splitOn: "id"
                ).Distinct().ToList();
            }
            foreach (var item in map)
            {
                Customer c = item.Value;
                System.Console.WriteLine(c);
                System.Console.WriteLine($"amount of orders {c.Orders.Count}");
            }
        }

        // splitON:
        // 
    }
}