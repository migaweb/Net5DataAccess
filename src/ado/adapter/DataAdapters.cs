using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Data.Common;
using code.ado.shared;
using System.Data.SQLite;
using System.Data;

namespace code.ado.adapter
{
    internal class DataAdapters
    {
        // private static string SELECT = "SELECT Id, Name, Lastname FROM Customers;";

        internal static void Run()
        {
            Read<SQLiteDataAdapter>(Constants.GetSqliteConnection());
            Read<Npgsql.NpgsqlDataAdapter>(Constants.GetPsqlConnection(), "psql");
        }

        private static DbDataAdapter GetAdapter<T>()
            where T : DbDataAdapter, new()
        {
            return new T();
        }

        private static void Read<T>(DbConnection connection, string provider="sqlite")
            where T : DbDataAdapter, new()
        {
            string select_query = "SELECT Id, Name, Lastname FROM Customers;";
            var data = new DataSet();
            using (connection)
            {
                connection.Open();
                using(var adapter = new Npgsql.NpgsqlDataAdapter())
                {
                    using (var cmd = (Npgsql.NpgsqlCommand)connection.CreateCommand())
                    {
                        cmd.CommandText = select_query;
                        adapter.SelectCommand = cmd;
                        adapter.Fill(data);
                    }
                }
            }

            foreach (DataRow row in data.Tables[0].Rows)
            {
                System.Console.WriteLine($"{row["Id"]} : {row["Name"]}");
            }
        }
    }
}