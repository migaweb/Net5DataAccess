using System;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Collections.Generic;

namespace nosql
{
    internal class Mongo
    {
        public static void Run()
        { 
            var bson = new BsonDocument()
            {
                ["Orders.1"] = new BsonDocument
                {
                    ["$exists"] = false
                }
            };
            Create();
            // CreateMany();
            // Update();
            // Delete();
            Read(new BsonDocument());
            // DeleteAllAndCollection();
        }

        private static IMongoCollection<BsonDocument> Connect()
        {
            string connection = "mongodb://localhost:27017";
            var client = new MongoClient(connection);
            IMongoDatabase db = client.GetDatabase("udemy"); // creates it
            var collection = db.GetCollection<BsonDocument>("customers");
            if (collection == null)
            {
                db.CreateCollection("customers");
            }

            return collection ?? db.GetCollection<BsonDocument>("customers");
        }

        private static void Create()
        {
            var collection = Connect();
            var customer = new Customer
            {
                Name = "first",
                LastName = "customer",
                Orders = new List<Order>
                {
                    new Order { Text = "order1", Costs = 42},
                    new Order { Text = "order4", Costs = 3 }
                }
            };
            collection.InsertOne(customer.ToBsonDocument());
        }

        private static void CreateMany()
        {
            var c2 = new Customer
            {
                Name = "second", LastName = "customer", Orders = new List<Order>()
            };
            var c3 = new BsonDocument
            {
                ["name"] = "third",
                ["lastname"] = "customer"
            };
            var collection = Connect();
            collection.InsertMany(new BsonDocument[]
            {
                c2.ToBsonDocument(), c3
            });
        }

        private static void Read(BsonDocument search)
        {
           var collection = Connect();
           using (var cursor = collection.FindSync(search))
           {
               while (cursor.MoveNext())
               {
                   var batch = cursor.Current;
                   foreach (var item in batch)
                   {
                       System.Console.WriteLine(item);
                       System.Console.WriteLine();
                   }
               }
           }
           System.Console.WriteLine("done reading!");
        }

        private static void Update()
        {
            string search = @"{""Orders.1"" : {$exists:false}}";
            var collection = Connect();

            var order = new Order { Text = "updated!", Costs = 42 };
            collection.UpdateMany(
                search,
                Builders<BsonDocument>.Update
                .Set("Orders", new List<Order> { order })
            );
        }

        private static void Delete()
        {
            var collection = Connect();
            string search = @"
            {
                ""Orders.Text"" :
                    { $eq : ""updated!"" }
            }
            "; // case sensitive!

            collection.DeleteMany(search);
        }

        private static void DeleteAllAndCollection()
        {
            string connection = "mongodb://localhost:27017";
            var client = new MongoClient(connection);
            IMongoDatabase db = client.GetDatabase("udemy");
            db.DropCollection("customers");
            client.DropDatabase("udemy");
        }
    }
}