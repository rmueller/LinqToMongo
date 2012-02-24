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
        public QueryableMongo()
        {
            Provider = new MongoQueryProvider();
            Expression = Expression.Constant(this);
        }

        public QueryableMongo(IQueryProvider provider, Expression expression)
        {
            if (provider == null)
            {
                throw new ArgumentNullException("provider");
            }

            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            if (!typeof(IQueryable<BsonDocument>).IsAssignableFrom(expression.Type))
            {
                throw new ArgumentOutOfRangeException("expression");
            }

            Provider = provider;
            Expression = expression;
        }

        #region IOrderedQueryable<BsonDocument> Members
        public IQueryProvider Provider { get; private set; }
        public Expression Expression { get; private set; }

        public Type ElementType
        {
            get { return typeof(BsonDocument); }
        }

        #endregion

        #region Enumerators
        public IEnumerator<BsonDocument> GetEnumerator()
        {
            return (Provider.Execute<IEnumerable<BsonDocument>>(Expression)).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (Provider.Execute<IEnumerable>(Expression)).GetEnumerator();
        }

        #endregion
    }
}