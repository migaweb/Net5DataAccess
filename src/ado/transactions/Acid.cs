using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Data.Common;
using code.ado.shared;

namespace code.ado.transaction
{
    internal class Acid
    {
        private const string INSERT_FAIL = 
            "INSERT INTO Orders (name, customerId) VALUES ('ordered', 111)";
        public static void Run()
        {
            // Except(Constants.GetSqliteConnection());
            // Except(Constants.GetPsqlConnection(), "psql");
            Transactional(Constants.GetSqliteConnection());
            Transactional(Constants.GetPsqlConnection(), "psql");
        }

        private static void Except(DbConnection connection, string provider = "sqlite")
        {
            System.Console.WriteLine($"provider: {provider} ");
            using (connection)
            {
                try
                {
                    connection.Open();
                    using (var cmd = connection.CreateCommand())
                    {
                        cmd.CommandText = INSERT_FAIL;
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (System.Exception ex)
                {
                    System.Console.WriteLine("threw!");
                    System.Console.WriteLine(ex.Message);
                    System.Console.WriteLine(ex.GetType().Name);
                }
                
            }
        }

        public static void Transactional(DbConnection connection, string provider = "sqlite")
        {
            // string UPDATE = "UPDATE Customers SET Name='abc' WHERE id=1";
            System.Console.WriteLine($"transaction with: {provider} ");

            DbTransaction transaction = null;
            using (connection)
            {
                try
                {
                    connection.Open();
                    transaction = connection.BeginTransaction();
                    
                    var cmd = connection.CreateCommand();
                    cmd.CommandText = "UPDATE Customers SET Name='abc' WHERE id=1";
                    int affected = cmd.ExecuteNonQuery();
                    System.Console.WriteLine($"UPDATE affected: {affected} rows");

                    var cmd2 = connection.CreateCommand();
                    // will fail!
                    cmd2.CommandText = "INSERT INTO Orders (name, customerId) VALUES ('ordered', 111)";
                    cmd2.ExecuteNonQuery();

                    System.Console.WriteLine("committing!");
                    transaction.Commit();
                }
                catch (System.Exception ex)
                {
                    try
                    {
                        System.Console.WriteLine("rolling back!");
                        transaction.Rollback();
                        System.Console.WriteLine(ex.Message);
                    }
                    catch (System.Exception ex2)
                    {
                        System.Console.WriteLine(ex2.Message);
                    }
                }
            }
            
        }
    }
}