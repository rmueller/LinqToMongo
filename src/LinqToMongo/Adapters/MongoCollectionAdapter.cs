using System;
using MongoDB.Bson;
using MongoDB.Driver;

namespace LinqToMongo.Adapters
{
    public class MongoCollectionAdapter
    {
        private readonly MongoCollection<BsonDocument> sourceCollectionField;

        public MongoCollectionAdapter()
        {
        }

        public MongoCollectionAdapter(MongoCollection<BsonDocument> source)
        {
            sourceCollectionField = source;
        }

        
        public MongoCursor<BsonDocument> FindAll()
        {
            return this.sourceCollectionField.FindAll();
        }

        public MongoCursor<BsonDocument> Find(IMongoQuery query)
        {
            return this.sourceCollectionField.Find(query);
        }

        public static implicit operator
            MongoCollectionAdapter(MongoCollection<BsonDocument> source)
        {
            return new MongoCollectionAdapter(source);
        }

    }
}