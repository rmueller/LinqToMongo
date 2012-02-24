using System;
using System.Linq;
using System.Linq.Expressions;
using MongoDB.Bson;

namespace LinqToMongo
{
    public class MongoQueryProvider : IQueryProvider
    {
        #region IQueryProvider Members

        public IQueryable CreateQuery(Expression expression)
        {
            return new QueryableMongo(this, expression);
        }

        public IQueryable<TResult> CreateQuery<TResult>(Expression expression)
        {
            return new QueryableMongo(this, expression) as IQueryable<TResult>;
        }

        public object Execute(Expression expression)
        {
            throw new NotImplementedException();
        }

        public TResult Execute<TResult>(Expression expression)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}