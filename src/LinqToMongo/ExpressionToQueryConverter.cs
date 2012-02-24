using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace LinqToMongo
{
    public static class ExpressionToQueryConverter
    {
        public static QueryComplete Convert(
            Expression<Func<BsonDocument, bool>> expression
            )
        {
            var visitor = new Visitor();
            visitor.Visit(expression);
            return (QueryComplete) visitor.ResultStack.Pop();
        }

        #region Nested type: Visitor

        private class Visitor : ExpressionVisitor
        {
            private readonly Stack<object> resultStackField = new Stack<object>();

            public Stack<object> ResultStack
            {
                get { return resultStackField; }
            }

            
            private object VisitAndProcess(Expression node)
            {
                Visit(node);
                return resultStackField.Pop();
            }

            protected override Expression VisitBinary(BinaryExpression node)
            {   
                resultStackField.Push(CreateQuery(
                    node.NodeType, 
                    VisitAndProcess(node.Left), 
                    VisitAndProcess(node.Right)
                    ));

                return node;
            }

            private IMongoQuery CreateQuery(ExpressionType type, object left, object right)
            {
                switch (type)
                {
                    case ExpressionType.Equal:
                        return Query.EQ((string) left, BsonValue.Create(right));
                    case ExpressionType.GreaterThan:
                        return Query.GT((string) left, BsonValue.Create(right));
                    case ExpressionType.LessThan:
                        return Query.LT((string) left, BsonValue.Create(right));
                    case ExpressionType.GreaterThanOrEqual:
                        return Query.GTE((string)left, BsonValue.Create(right));
                    case ExpressionType.LessThanOrEqual:
                        return Query.LTE((string)left, BsonValue.Create(right));

                    case ExpressionType.AndAlso:
                        return Query.And((IMongoQuery) left, (IMongoQuery) right);
                    case ExpressionType.OrElse:
                        return Query.Or((IMongoQuery) left, (IMongoQuery) right);
                }

                throw new NotSupportedException(
                    string.Format("NodeType '{0}' is not supported!", type)
                    );
            }

            protected override Expression VisitConstant(ConstantExpression node)
            {
                resultStackField.Push(node.Value);
                return base.VisitConstant(node);
            }
        }

        #endregion
    }
}