using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MongoDB.Bson;

namespace LinqToMongo
{
    public class QueryableMongo : IOrderedQueryable<BsonDocument>
    {
        #region IOrderedQueryable<BsonDocument> Members

        public IEnumerator<BsonDocument> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public Type ElementType
        {
            get { throw new NotImplementedException(); }
        }

        public Expression Expression
        {
            get { throw new NotImplementedException(); }
        }

        public IQueryProvider Provider
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }
}