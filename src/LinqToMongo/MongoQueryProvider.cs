using System;
using System.Linq;
using System.Linq.Expressions;

using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Driver.Builders;

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

        internal IMongoQuery GetMongoWhere(Expression expression)
        {
            var visitor = new Visitor();
            visitor.Visit(expression);
            return visitor.Where;
        }

        #region Nested type: Visitor

        private class Visitor : ExpressionVisitor
        {
            public IMongoQuery Where { get; private set; }

            protected override Expression VisitMethodCall(
                MethodCallExpression node
                )
            {
                if (node.Method.Name == "Where")
                {
                    Visit(node.Arguments[0]);
                    var filter = ((UnaryExpression) node.Arguments[1]).Operand;
                    Where = Query.And(
                        Where,
                        ExpressionToQueryConverter.Convert((Expression<Func<BsonDocument, bool>>) filter)
                        );
                    return node;
                }

                throw new NotSupportedException(
                    string.Format("'{0}' method is not supported", node.Method.Name)
                    );
            }
        }

        #endregion
    }
}