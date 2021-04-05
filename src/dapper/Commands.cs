using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using code.ado.shared;
using Dapper;
using System.Data;

namespace dapper
{
    internal class Commands
    {
        public static void Run()
        {
            // Insert();
            // InsertMany();
            // Update();
            Delete();
        }

        private static string insert = @"
            INSERT INTO Customers (name, lastname) VALUES (@name, @lastname);
        ";

        private static IDbConnection GetConnection()
        {
            return new Npgsql.NpgsqlConnection(Constants.PSQL_CONNECTION);
        }

        private static void Insert()
        {
            string sqlInsert = @"
            INSERT INTO Customers (name, lastname) VALUES (@name, @lastname);
        ";
            using (var connection = GetConnection())
            {
                int affectedRows = connection.Execute(
                    sqlInsert,
                    new { name = "inserted", lastname = "delete_me"}
                );
            }
        }

         private static void Update()
        {
            using (var connection = GetConnection())
            {
                string update = "UPDATE customers SET name = @update WHERE name = 'forUpdate'";
                int affected = connection.Execute(update,
                    new { update = "updated!" }
                );
                System.Console.WriteLine($"affected: {affected}");
            }
        }

        private static void Delete()
        {
            using (var connection = GetConnection())
            {
                string delete = "DELETE FROM customers Where lastname = 'delete_me'";
                int affected = connection.Execute(delete);
                System.Console.WriteLine($"affected: {affected}");
            }
        }

        private static void InsertMany()
        {
            using (var connection = GetConnection())
            {
                int affectedRows = connection.Execute(
                    insert,
                    new []
                    {
                        new { name = "inserted", lastname = "delete_me"},
                        new { name = "forUpdate", lastname = "delete_me"},
                        new { name = "forUpdate", lastname = "delete_me"},
                    }
                );
                System.Console.WriteLine($"affected: {affectedRows}");
            }
        }

       
    }
}