using System;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace LinqToMongo.Demo
{
    internal class Program
    {
        static void Main()
        {
            var server = MongoServer.Create("mongodb://localhost");
            server.Connect();

            var db = server.GetDatabase("demo");
            var collection = db.GetCollection("people");

            collection.Insert(new BsonDocument
                                  {
                                      {"name", "Mary"},
                                      {"age", 35}
                                  });

            collection.Insert(new BsonDocument
                                  {
                                      {"name", "Jonny"},
                                      {"age", 32}
                                  });

            collection.Insert(new BsonDocument
                                  {
                                      {"name", "Kant"},
                                      {"age", 13}
                                  });

            var q = from d in collection.AsQueryable()
                    where d["age"] > 30
                    orderby d["age"] ascending 
                    select d;

            var q2 = collection
                .Find(Query.GT("age", 20))
                .SetSortOrder("age");
           
            foreach (var p in q)
                Console.WriteLine("{0} (Age={1})", p["name"], p["age"]);

            server.DropDatabase("demo");

            server.Disconnect();

            Console.ReadLine();
        }
    }
}