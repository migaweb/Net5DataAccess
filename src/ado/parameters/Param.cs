using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using code.ado.shared;
using System.Data.Common;
using System.Data.SQLite;

namespace code.ado.parameters
{
    internal class Param
    {
        private static string SQL = @"
                        SELECT Count(o.Id), c.Id FROM Customers c
                        INNER JOIN Orders o
                        On o.CustomerId = c.Id
                        WHERE c.Id < @id
                        GROUP BY c.id;
                    ";
        public static void Run()
        {
            RunParams<SQLiteParameter>(Constants.GetSqliteConnection());
            RunParams<Npgsql.NpgsqlParameter>(Constants.GetPsqlConnection(), "psql");
        }

        private static DbParameter GetParameter<T>()
            where T : DbParameter, new()
        =>  new T()
        {
            ParameterName = "id",
            Value = 3
        };

        private static string CountToReader(string provider)
            => provider == "sqlite" ? "Count(o.id)" : "count";

        private static void RunParams<T>(DbConnection dbConnection, string provider="sqlite")
            where T : DbParameter, new()
        {
            System.Console.WriteLine($"{provider}");
            using (dbConnection)
            {
                dbConnection.Open();
                using (var cmd = dbConnection.CreateCommand())
                {
                    cmd.CommandText = SQL;
                    DbParameter param = GetParameter<T>();
                    cmd.Parameters.Add(param);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string line = $"count:{reader[CountToReader(provider)]}, "
                            + $"c.id: {reader["id"]}";
                            System.Console.WriteLine(line);
                        }
                    }
                }
                System.Console.WriteLine("done with reading");
            }
        }
    }
}