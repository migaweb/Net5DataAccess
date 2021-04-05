using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SQLite;
using code.ado.shared;

namespace code.ado.readers
{
    internal class DataReaders // forward only and readonly 
    {
        // select
        // inner join
        private const string SELECT = "SELECT id,name,lastname FROM Customers";
        private const string JOIN = @"SELECT c.Id, o.Id as orderId From Customers c
                                      INNER JOIN orders o
                                      ON o.CustomerId = c.Id
                                     ";
        public static void Run()
        {
            ReadAll(new SQLiteConnection(Constants.SQLITE_CONNECTION));
            ReadAll(new Npgsql.NpgsqlConnection(Constants.PSQL_CONNECTION), "psgl");
        }

        private static void ReadAll(DbConnection connection, string provider = "sqlite")
        {
            System.Console.WriteLine($"provider: {provider} ");
            using (connection)
            {
                connection.Open();
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = JOIN;
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string line = $"c.Id:{reader["id"]}, order.Id:{reader["orderId"]}";
                            System.Console.WriteLine(line);
                        }
                    }
                    System.Console.WriteLine("reader is done!");
                }
            }
        }
    }
}