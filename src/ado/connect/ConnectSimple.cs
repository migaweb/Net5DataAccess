using System;
using System.Data.Common;
using System.Data.SQLite;
using System.Linq;
using Npgsql;

namespace code.ado.connect
{
    internal class ConnectSimple
    {
        public static void Run()
        {
            // just example code for simple way to connect to a database with ADO.NET
        }

        private static void ConnectNpgsql()
        {
            var connString = $"Server=localhost;Database=udemy;User Id=udemy;Port=5433;";
            using (var connection = new NpgsqlConnection(connString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT COUNT(*) FROM customers;";
                var result = command.ExecuteScalar();
                System.Console.WriteLine(result);
            }
        }

        private static void ConnectSqlite()
        {
            var connString = $"DataSource={Environment.CurrentDirectory}/data/sqlite/ecommerce.db;Version=3;";
            var connection = new SQLiteConnection(connString);

            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = "SELECT COUNT(*) FROM customers;";
            var result = command.ExecuteScalar();
            System.Console.WriteLine(result);

            connection.Close();
        }
    }
}