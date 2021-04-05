using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using code.ado.shared;
using Dapper;
using System.Data;
using System.Data.SQLite;

namespace dapper
{
    internal class UseDynamicParams
    {
        private static IDbConnection Connect => new SQLiteConnection(Constants.SQLITE_CONNECTION);
        public static void Run()
        {
            // ReadWithInParameters();
            InsertWithInParameters();
        }


        private static void ReadWithInParameters()
        {
           string sql = "SELECT * FROM customers WHERE Id > @Id";
           var parameters = new DynamicParameters();
           parameters.Add("Id", 1, DbType.Int32, ParameterDirection.Input);
           using (var connection = Connect)
           {
               var customers = connection.Query<Customer>(sql, parameters).ToList();
               System.Console.WriteLine(string.Join(",", customers));
           }
        }

        private static void InsertWithInParameters()
        {
           string sql = "INSERT INTO customers VALUES (Null, @Name, @Lastname);";
           var parameters = new DynamicParameters();
           parameters.Add("Name", "Customer", DbType.String, ParameterDirection.Input);
           parameters.Add("Lastname", "DynamicParameters", DbType.String, ParameterDirection.Input);
           using (var connection = Connect)
           {
               var value = connection.Execute(sql, parameters);
               var customers = connection.QueryFirst<int>("SELECT COUNT(*) FROM customers;");
               System.Console.WriteLine($"there are now {customers} customers");
           }
        }
    }
}