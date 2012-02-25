﻿using System;
using System.Linq;
using System.Linq.Expressions;

using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Driver.Builders;
using System.Diagnostics;

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
            var visitor = new Visitor();
            visitor.Visit(expression);

            var presult = visitor.Where == null ? visitor.Collection.FindAll() : visitor.Collection.Find(visitor.Where);
            var sortBy = SortBy.Ascending("age");
            
            return (TResult) (object) presult.SetSortOrder(sortBy);
        }

        #endregion

        internal IMongoQuery GetMongoWhere(Expression expression)
        {
            var visitor = new Visitor();
            visitor.Visit(expression);
            return visitor.Where;
        }

        internal SortByBuilder GetMongoSortByBuilder(Expression expression)
        {
            var visitor = new Visitor();
            visitor.Visit(expression);
            return visitor.SortByBuilder;
        }

        #region Nested type: Visitor

        private class Visitor : ExpressionVisitor
        {
            public IMongoQuery Where { get; private set; }
            public MongoCollection<BsonDocument> Collection { get; private set; }
            public SortByBuilder SortByBuilder { get; private set;  }

            public Visitor()
            {
                SortByBuilder = new SortByBuilder();
                //Where = new BsonDocument();
            }

            protected override Expression VisitConstant(ConstantExpression node)
            {
                if (node.Value is QueryableMongo)
                    Collection = (node.Value as QueryableMongo).Collection;

                return base.VisitConstant(node);
            }

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

                if (node.Method.Name == "OrderBy")
                {
                    Visit(node.Arguments[0]);
                    var unary = (UnaryExpression) node.Arguments[1];
                    var o = (Expression<Func<BsonDocument, BsonValue>>) unary.Operand;
                    var m = (MethodCallExpression) o.Body;
                    var c = (ConstantExpression) m.Arguments[0];
                    this.SortByBuilder.Ascending(c.Value.ToString());
                    return node;
                }

                if (node.Method.Name == "OrderByDescending")
                {
                    Visit(node.Arguments[0]);
                    var unary = (UnaryExpression)node.Arguments[1];
                    var o = (Expression<Func<BsonDocument, BsonValue>>)unary.Operand;
                    var m = (MethodCallExpression)o.Body;
                    var c = (ConstantExpression)m.Arguments[0];
                    this.SortByBuilder.Descending(c.Value.ToString());
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