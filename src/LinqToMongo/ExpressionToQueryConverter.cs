using System;
using System.Linq.Expressions;
using System.Diagnostics;
using System.Collections.Generic;

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

        private class Visitor : ExpressionVisitor
        {
            readonly Stack<object> resultStackField = new Stack<object>();
            public Stack<object> ResultStack { get { return resultStackField; } }

            public override Expression Visit(Expression node)
            {
                Debug.Print("{0}: {1}", node.GetType().ToString(),  node.ToString());
                return base.Visit(node);
            }

            protected override Expression VisitBinary(BinaryExpression node)
            {
                Debug.Print("BINARY {0} --> {1}: {2}", node.NodeType,node.GetType().ToString(), node.ToString());
                
                Visit(node.Left);
                var left = resultStackField.Pop();
                Visit(node.Right);
                var right = resultStackField.Pop();
                
                if (node.NodeType == ExpressionType.Equal)
                    resultStackField.Push(Query.EQ((string)left, (string)right));
                else if (node.NodeType == ExpressionType.OrElse)
                    resultStackField.Push(Query.Or((IMongoQuery) left, (IMongoQuery) right));
                else
                    throw new NotSupportedException(string.Format("NodeType '{0}' is not supported!", node.NodeType));

                return node;
            }

            protected override Expression VisitConstant(ConstantExpression node)
            {
                Debug.Print("PUSHED {0}", node.Value);
                resultStackField.Push(node.Value);
                return base.VisitConstant(node);
            }
        }
    }
}