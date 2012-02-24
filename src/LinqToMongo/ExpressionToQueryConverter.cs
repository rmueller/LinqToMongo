using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;
using System.Linq.Expressions;
using MongoDB.Bson;
using MongoDB.Driver.Builders;

namespace LinqToMongo
{
    public static class ExpressionToQueryConverter
    {
        public static QueryComplete Convert(
            Expression<Func<BsonDocument, bool>> expression
            )
        {
            return Query.EQ("name", "John");
        }

    }
}
