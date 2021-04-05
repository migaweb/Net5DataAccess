using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SQLite;
using Npgsql;

namespace code.ado.shared
{
    internal static class Constants
    {
        internal static string SQLITE_CONNECTION = $"DataSource={Environment.CurrentDirectory}/data/sqlite/ecommerce.db;Version=3;foreignkeys=true";
        internal static string PSQL_CONNECTION = "Server=localhost;Database=udemy;User Id=udemy;Port=5433";

        internal static DbConnection GetSqliteConnection()
        {
            return new SQLiteConnection(SQLITE_CONNECTION);
        }

        internal static DbConnection GetPsqlConnection()
        {
            return new NpgsqlConnection(PSQL_CONNECTION);
        }

        
    }
}