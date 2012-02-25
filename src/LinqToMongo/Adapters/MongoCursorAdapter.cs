using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace LinqToMongo.Adapters
{
    public class MongoCursorAdapter : IEnumerable<BsonDocument>
    {
        private readonly MongoCursor<BsonDocument> sourceCursorField;

        public MongoCursorAdapter()
        {
             
        }

        public MongoCursorAdapter(MongoCursor<BsonDocument> source)
        {
            sourceCursorField = source;
        }

        public IEnumerator<BsonDocument> GetEnumerator()
        {
            return sourceCursorField.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return sourceCursorField.GetEnumerator();
        }

        public MongoCursorAdapter SetSortOrder(SortByBuilder sortBy)
        {
            return sourceCursorField.SetSortOrder(sortBy);
        }

        public static implicit operator 
            MongoCursorAdapter(MongoCursor<BsonDocument> source)
        {
            return new MongoCursorAdapter(source);
        }
    }
}
