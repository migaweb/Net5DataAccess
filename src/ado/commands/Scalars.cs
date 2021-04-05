using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using code.ado.shared;
using System.Data.Common;

namespace code.ado.commands
{
    internal class Scalars
    {
        const string INSERT = "INSERT INTO Orders (name, customerId)" 
            + " VALUES ('order1', 1)";
        const string UPDATE = "UPDATE Orders SET name = 'updated' WHERE Id >= 5";
        const string DELETE = "DELETE FROM Orders WHERE name = 'updated'";

        public static void Run()
        {
            Insert(Constants.GetSqliteConnection());
            Insert(Constants.GetPsqlConnection(), provider:"psql");

            // InsertFails();
        }

        private static void Insert(DbConnection dbConnection, string provider="sqlite")
        {
            using (dbConnection)
            {
                dbConnection.Open();
                using (var cmd = dbConnection.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM Orders WHERE name = 'updated'";
                    int scalar = cmd.ExecuteNonQuery();
                    System.Console.WriteLine($"rows affected: {scalar}");
                }
            }
        }
    }
}