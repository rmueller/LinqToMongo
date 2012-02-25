using System.Linq;

using NUnit.Framework;
using MongoDB.Driver.Builders;
using SharpTestsEx;

namespace LinqToMongo.Tests
{
    [TestFixture]
    public class QueryableMongoTests
    {
        [Test]
        public void QueryUsingWhereAsExtensionMethod()
        {
            var setup = QueryableMongo.Create(null).Where(item => item["age"] > 10);

            var q = 
               Query.GT("age", 10)
              ;

            ((QueryableMongo)setup).GetQuery()
                .ToString()
                .Should().Be(q.ToString());
        }

        [Test]
        public void QueryUsingWhereAsLinqSyntax()
        {
            var setup = from item in QueryableMongo.Create(null)
                        where item["age"] > 10
                        select item;

            var q = 
               Query.GT("age", 10)
               ;

            ((QueryableMongo)setup).GetQuery()
                .ToString()
                .Should().Be(q.ToString());
        }

        [Test]
        public void AndQueryUsingWhereAsLinqSyntax()
        {
            var setup = from item in QueryableMongo.Create(null)
                        where item["age"] > 10 && item["age"] <= 25
                        select item;

            var q = Query.And(
               Query.GT("age", 10),
               Query.LTE("age", 25)
               );

            ((QueryableMongo)setup).GetQuery()
                .ToString()
                .Should().Be(q.ToString());
        }

        [Test]
        public void AndQueryUsingWhereAsChainedExtensionMethods()
        {
            var setup = QueryableMongo.Create(null)
                .Where(item => item["age"] > 10)
                .Where(item => item["age"] <= 25);

            var q = Query.And(
               Query.GT("age", 10),
               Query.LTE("age", 25)
               );

            ((QueryableMongo)setup).GetQuery()
                .ToString()
                .Should().Be(q.ToString());
        }

        [Test]
        public void AndQueryUsingWhereAsChainedExtensionMethods2()
        {   
            var setup = 
                Queryable.Where(
                    Queryable.Where(
                        QueryableMongo.Create(null), 
                        item => item["age"] > 10), 
                    item => item["age"] <= 25
                );

            var q = Query.And(
               Query.GT("age", 10),
               Query.LTE("age", 25)
               );

            ((QueryableMongo)setup).GetQuery()
                .ToString()
                .Should().Be(q.ToString());
        }

        [Test]
        public void BasicOrderByUsingExtensionMethods ()
        {
            var setup = QueryableMongo.Create(null)
                .OrderBy(d => d["name"]);

            var sortBy = ((QueryableMongo) setup).GetSortBy();
            sortBy.ToString().Should().Be("{ \"name\" : 1 }");
        }

        [Test]
        public void BasicOrderByUsingLinqSyntax()
        {
            var setup = from d in QueryableMongo.Create(null)
                        orderby d["name"]
                        select d;

            var sortBy = ((QueryableMongo)setup).GetSortBy();
            sortBy.ToString().Should().Be("{ \"name\" : 1 }");
        }

        [Test]
        public void BasicOrderByDescendingUsingExtensionMethods()
        {
            var setup = QueryableMongo.Create(null)
                .OrderByDescending(d => d["name"]);

            var sortBy = ((QueryableMongo)setup).GetSortBy();
            sortBy.ToString().Should().Be("{ \"name\" : -1 }");
        }

        [Test]
        public void BasicOrderByDescendingUsingLinqSyntax()
        {
            var setup = from d in QueryableMongo.Create(null)
                        orderby d["name"] descending 
                        select d;

            var sortBy = ((QueryableMongo)setup).GetSortBy();
            sortBy.ToString().Should().Be("{ \"name\" : -1 }");
        }

    }
}