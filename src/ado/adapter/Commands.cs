using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using code.ado.shared;
using System.Data.Common;
using System.Data.SQLite;
using System.Data;

namespace code.ado.adapter
{
    internal class Commands
    {
        const string INSERT = "INSERT INTO Orders (id, name, customerId)" 
            + " VALUES (42, 'order1', 1)";
        const string UPDATE = "UPDATE Orders SET name = 'updated' WHERE id =42";
        const string DELETE = "DELETE FROM Orders WHERE name = 'updated'";
        public static void Run()
        {
            Cmd<SQLiteDataAdapter>(Constants.GetSqliteConnection());
            Cmd<Npgsql.NpgsqlDataAdapter>(Constants.GetPsqlConnection(), "psql");
        }

        private static DbDataAdapter GetAdapter<T>()
            where T : DbDataAdapter, new()
        {
            return new T();
        }

        private static void Cmd<T>(DbConnection connection, string provider="sqlite")
            where T : DbDataAdapter, new()
        {
            System.Console.WriteLine("provider: " + provider);
            using (connection)
            {
                connection.Open();
                var adapter = GetAdapter<T>();
                var cmd = connection.CreateCommand();
                cmd.CommandText = INSERT;
                adapter.InsertCommand = cmd;
                int affected = adapter.InsertCommand.ExecuteNonQuery();
                System.Console.WriteLine($"INSERT affected {affected}");

                var update = GetAdapter<T>();
                cmd = connection.CreateCommand();
                cmd.CommandText = UPDATE;
                update.UpdateCommand = cmd;
                System.Console.WriteLine($"UPDATE affected {update.UpdateCommand.ExecuteNonQuery()}");

                var delete = GetAdapter<T>();
                cmd = connection.CreateCommand();
                cmd.CommandText = DELETE;
                delete.DeleteCommand = cmd;
                System.Console.WriteLine($"DELETE affected {delete.DeleteCommand.ExecuteNonQuery()}");


            }
        }
    }
}