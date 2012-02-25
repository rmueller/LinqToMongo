using MongoDB.Bson;
using MongoDB.Driver;

namespace LinqToMongo
{
    public static class MongoExtensions
    {
        public static QueryableMongo AsQueryable(this MongoCollection<BsonDocument> that)
        {
            return new QueryableMongo(that);
        }
    }
}