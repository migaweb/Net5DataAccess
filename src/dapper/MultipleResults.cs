using System.Linq;
using code.ado.shared;
using Dapper;
using System.Data;
using System.Data.SQLite;

namespace dapper
{
    internal class MultipleResults
    {
        public static void Run()
        {
            ReadSingle();
        }

        private static IDbConnection Connect => new SQLiteConnection(Constants.SQLITE_CONNECTION);

        private static void ReadSingle()
        {
            string sql = "SELECT * FROM customers; SELECT * FROM orders";
            using (var multi = Connect.QueryMultiple(sql))
            {
                var customers = multi.Read<Customer>();
                var orders = multi.Read<Order>();

                System.Console.WriteLine(customers.Count());
                System.Console.WriteLine(orders.Count());
            }
        }

        
    }
}