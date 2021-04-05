using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using code.ef.Models;
using Microsoft.EntityFrameworkCore;

namespace code.ef
{
    internal class Commands
    {
        public static void Run()
        {
            // SelectAll();
            // InnerJoin();
            // Insert();
            // Update();
            // Delete();
            Raw();
        }

        private static void Raw()
        {
            string raw = @"
                SELECT * FROM customers;
            ";
            using (var context = new udemyContext())
            {
                var customers = context.Customers.FromSqlRaw(raw);
                foreach (var item in customers)
                {
                    System.Console.WriteLine(item);
                }
            }
        }

        private static void Insert()
        {
            // INSERT INTO Customers (name, lastname) VALUES ('customer', 'from ef');
            var customer = new Customers
            {
                Name = "customer",
                Lastname = "from ef"
            };
            System.Console.WriteLine(customer);
            using (var context = new udemyContext())
            {
                context.Add(customer);
                //unit of work:
                context.SaveChanges();
            }
        }

        private static void Update()
        {
            using (var context = new udemyContext())
            {
                Customers fromEf = context.Set<Customers>().Find(6);
                fromEf.Name = "updated";
                context.SaveChanges();
            }
        }

        private static void Delete()
        {
            using (var context = new udemyContext())
            {
                Customers updated = context.Set<Customers>()
                                            .Where(c => c.Id == 6)
                                            .FirstOrDefault();
                context.Remove(updated);
                context.SaveChanges();
            }
        }

        private static void SelectAll()
        {
           // SELECT * FROM customers
           using (var context = new udemyContext())
           {
               var customers = context.Customers;
               foreach (var item in customers)
               {
                   System.Console.WriteLine(item);
               }
           }
        }

        private static void InnerJoin()
        {
            // SELECT * FROM customers c
            // INNER JOIN orders o
            // on c.Id = o.CustomerId
            // where c.Id < 3;
            using (var context = new udemyContext())
            {
                var customers = context.Customers
                                        .Where(c => c.Id < 3)
                                        .Include(c => c.Orders);
                foreach (var item in customers)
                {
                    System.Console.WriteLine(item);
                    System.Console.WriteLine($"amount of orders: {item.Orders.Count}");
                }
            }
        }
    }
}