using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using code.ado.shared;
using Dapper;
using System.Data;
using System.Data.SQLite;
using System.Data.Common;

namespace dapper
{
    internal class Parameters
    {
        // single and many
        public static void Run()
        {
            // anonymous
            // dynamic
            InsertAList();
        }

        private static string insert_order = @"INSERT INTO orders (Id, Name, CustomerId) VALUES (Null, @Name, @customerId)";
        private static string update = @"UPDATE orders SET name = @Name WHERE id > 5";
        private static IDbConnection GetConnection()
        {
            return new SQLiteConnection(Constants.SQLITE_CONNECTION);
        }

        private static List<Order> GetList() 
            =>  new List<Order>
            {
                new Order { Name= "from_list_1", CustomerId = 1 },
                new Order { Name= "from_list_2", CustomerId = 1 },
                new Order { Name= "from_list_3", CustomerId = 2 }
            };

        private static void InsertAList()
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                int result = connection.Execute(insert_order, GetList());

                System.Console.WriteLine($"inserted: {result}");
                UpdateAfterwards(connection);
            }
        }

        private static void UpdateAfterwards(IDbConnection connection)
        {
            var list = GetList().Select(x => new Order{ Name = "updated", CustomerId = x.CustomerId });
            connection.Execute(update, list);
            System.Console.WriteLine("updated!");
        }
    }
}