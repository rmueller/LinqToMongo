using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using MongoDB.Driver;

namespace LinqToMongo.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = MongoServer.Create("mongodb://localhost");
            server.Connect();

            var db = server.GetDatabase("demo");
            var collection = db.GetCollection("people");

            collection.Insert(new BsonDocument {
                                      {"name", "Jonny"},
                                      {"age", 32}
                                  });

            collection.Insert(new BsonDocument {
                                      {"name", "Mary"},
                                      {"age", 35}
                                  });

            collection.Insert(new BsonDocument {
                                      {"name", "Kant"},
                                      {"age", 13}
                                  });

            var q = from d in QueryableMongo.Create(collection)
                    where d["age"] > 30
                    select d;

            foreach (var p in q)
                Console.WriteLine("{0} (Age={1})", p["name"], p["age"]);

            server.DropDatabase("demo");

            server.Disconnect();

            Console.ReadLine();
        }
    }
}
