using System;
using System.Data.Common;
using System.Data.SQLite;
using System.Linq;
using Npgsql;

namespace code.ado.connect
{
    internal class Connecting
    {
        private static string sqlite_connect = 
            $"DataSource={Environment.CurrentDirectory}/data/sqlite/ecommerce.db;Version=3;";

        private static string psql_connect = 
            $"Server=localhost;Database=udemy;User Id=udemy;Port=5433;";
        public static void Run()
        {
            // connect
            // set off query
            // print
            ConnectSqlite();
            // ConnectNpgsql();
        }

        private static void RunSelectAll(DbConnection connection, string provider="sqlite")
        {
            using (connection)
            {
                connection.Open(); 
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "SELECT COUNT(*) FROM Customers;"; // yields 4
                    long result = (long)cmd.ExecuteScalar();

                    System.Console.WriteLine($"{provider}: amount of customers: {result}");
                }
            }
        }

        private static void ConnectSqlite()
        {
           RunSelectAll(new SQLiteConnection(sqlite_connect));
        }

        private static void ConnectNpgsql()
        {
           RunSelectAll(new NpgsqlConnection(psql_connect), provider:"psql");
        }
    }
}