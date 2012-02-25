using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver.Builders;
using NUnit.Framework;
using SharpTestsEx;

namespace LinqToMongo.Tests
{
    [TestFixture]
    public class QueryableMongoTests
    {
        [Test]
        public void AndQueryUsingWhereAsChainedExtensionMethods()
        {
            IQueryable<BsonDocument> setup = QueryableMongo.Create(null)
                .Where(item => item["age"] > 10)
                .Where(item => item["age"] <= 25);

            QueryComplete q = Query.And(
                Query.GT("age", 10),
                Query.LTE("age", 25)
                );

            ((QueryableMongo) setup).GetQuery()
                .ToString()
                .Should().Be(q.ToString());
        }

        [Test]
        public void AndQueryUsingWhereAsChainedExtensionMethods2()
        {
            IQueryable<BsonDocument> setup =
                QueryableMongo.Create(null).Where(item => item["age"] > 10).Where(item => item["age"] <= 25
                    );

            QueryComplete q = Query.And(
                Query.GT("age", 10),
                Query.LTE("age", 25)
                );

            ((QueryableMongo) setup).GetQuery()
                .ToString()
                .Should().Be(q.ToString());
        }

        [Test]
        public void AndQueryUsingWhereAsLinqSyntax()
        {
            IQueryable<BsonDocument> setup = from item in QueryableMongo.Create(null)
                                             where item["age"] > 10 && item["age"] <= 25
                                             select item;

            QueryComplete q = Query.And(
                Query.GT("age", 10),
                Query.LTE("age", 25)
                );

            ((QueryableMongo) setup).GetQuery()
                .ToString()
                .Should().Be(q.ToString());
        }

        [Test]
        public void BasicOrderByDescendingUsingExtensionMethods()
        {
            IOrderedQueryable<BsonDocument> setup = QueryableMongo.Create(null)
                .OrderByDescending(d => d["name"]);

            SortByBuilder sortBy = ((QueryableMongo) setup).GetSortBy();
            sortBy.ToString().Should().Be("{ \"name\" : -1 }");
        }

        [Test]
        public void BasicOrderByDescendingUsingLinqSyntax()
        {
            IOrderedQueryable<BsonDocument> setup = from d in QueryableMongo.Create(null)
                                                    orderby d["name"] descending
                                                    select d;

            SortByBuilder sortBy = ((QueryableMongo) setup).GetSortBy();
            sortBy.ToString().Should().Be("{ \"name\" : -1 }");
        }

        [Test]
        public void BasicOrderByUsingExtensionMethods()
        {
            IOrderedQueryable<BsonDocument> setup = QueryableMongo.Create(null)
                .OrderBy(d => d["name"]);

            SortByBuilder sortBy = ((QueryableMongo) setup).GetSortBy();
            sortBy.ToString().Should().Be("{ \"name\" : 1 }");
        }

        [Test]
        public void BasicOrderByUsingLinqSyntax()
        {
            IOrderedQueryable<BsonDocument> setup = from d in QueryableMongo.Create(null)
                                                    orderby d["name"]
                                                    select d;

            SortByBuilder sortBy = ((QueryableMongo) setup).GetSortBy();
            sortBy.ToString().Should().Be("{ \"name\" : 1 }");
        }

        [Test]
        public void BasicThenByDescendingUsingLinqSyntax()
        {
            IOrderedQueryable<BsonDocument> setup = from d in QueryableMongo.Create(null)
                                                    orderby d["name"] , d["age"] descending
                                                    select d;

            SortByBuilder sortBy = ((QueryableMongo) setup).GetSortBy();
            sortBy.ToString().Should().Be("{ \"name\" : 1, \"age\" : -1 }");
        }

        [Test]
        public void BasicThenByUsingExtensionMethods()
        {
            IOrderedQueryable<BsonDocument> setup = QueryableMongo.Create(null)
                .OrderBy(d => d["name"])
                .ThenBy(d => d["age"]);

            SortByBuilder sortBy = ((QueryableMongo) setup).GetSortBy();
            sortBy.ToString().Should().Be("{ \"name\" : 1, \"age\" : 1 }");
        }

        [Test]
        public void BasicThenByUsingLinqSyntax()
        {
            IOrderedQueryable<BsonDocument> setup = from d in QueryableMongo.Create(null)
                                                    orderby d["name"] , d["age"]
                                                    select d;

            SortByBuilder sortBy = ((QueryableMongo) setup).GetSortBy();
            sortBy.ToString().Should().Be("{ \"name\" : 1, \"age\" : 1 }");
        }

        [Test]
        public void QueryUsingWhereAsExtensionMethod()
        {
            IQueryable<BsonDocument> setup = QueryableMongo.Create(null).Where(item => item["age"] > 10);

            QueryConditionList q =
                Query.GT("age", 10)
                ;

            ((QueryableMongo) setup).GetQuery()
                .ToString()
                .Should().Be(q.ToString());
        }

        [Test]
        public void QueryUsingWhereAsLinqSyntax()
        {
            IQueryable<BsonDocument> setup = from item in QueryableMongo.Create(null)
                                             where item["age"] > 10
                                             select item;

            QueryConditionList q =
                Query.GT("age", 10)
                ;

            ((QueryableMongo) setup).GetQuery()
                .ToString()
                .Should().Be(q.ToString());
        }
    }
}