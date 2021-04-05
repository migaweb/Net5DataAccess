using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using StackExchange.Redis;
using Newtonsoft.Json;

namespace nosql.redis
{
    internal class Commands
    {
        public static void Run()
        {
            // Create();
            // Read();
            // Update();
            Delete();
        }

        private static IDatabase Connect()
        {
            var redis = ConnectionMultiplexer.Connect("localhost");
            return redis.GetDatabase();
        }

        private static void Create()
        {
            var db = Connect();
            var customer = new Customer
            {
                Name = "first",
                LastName = "customer",
                Orders = new List<Order>()
                {
                    new Order { Text = "order1", Costs = 42 }
                }
            };
            db.StringSet("customer:1", customer.ToString("redis", null));
        }

        private static void Read()
        {
            var db = Connect();
            string key = "customer:1";
            if (db.KeyExists(key))
            {
                string result = db.StringGet(key);
                var json = JsonConvert.DeserializeObject<Customer>(result);
                System.Console.WriteLine(json.ToString("output", null));
            }
            else
            {
                System.Console.WriteLine("key not found " + key);
            }
        }

        private static void Update()
        {
            string key = "customer:2";
            var db = Connect();

            string result = db.StringGet(key);
            var json = JsonConvert.DeserializeObject<Customer>(result);
            json.Orders = new List<Order>()
            {
                new Order { Text = "updated", Costs = 42}
            };
            db.StringSet(key, JsonConvert.SerializeObject(json));

            System.Console.WriteLine(db.StringGet(key));
        }

        private static void Delete()
        {
            var redis = ConnectionMultiplexer.Connect("localhost");
            var db = redis.GetDatabase();
            var server = redis.GetServer("localhost", 6379);

            var result = server.Keys(db.Database, "customer:?");

            foreach (var item in result)
            {
                db.KeyDelete(item);
            }
            System.Console.WriteLine(server.Keys(db.Database, "customer:?"));
        }
    }
}