using MongoDB.Driver.Builders;
using NUnit.Framework;
using SharpTestsEx;

namespace LinqToMongo.Tests
{
    [TestFixture]
    public class ExpressionToQueryConverterTests
    {
        [Test]
        public void SimpleEQ()
        {
            QueryComplete target = ExpressionToQueryConverter.Convert(d => d["name"] == "John");
            QueryComplete q = Query.EQ("name", "John");
            target
                .ToString()
                .Should().Be(q.ToString());
        }

        [Test]
        public void SimpleOR()
        {
            QueryComplete target = ExpressionToQueryConverter.Convert(
                d => d["name"] == "John" || d["name"] == "Mary"
                );

            QueryComplete q = Query.Or(
                Query.EQ("name", "John"),
                Query.EQ("name", "Mary")
                );
            
            target
                .ToString()
                .Should().Be(q.ToString());
        }

        [Test]
        public void SimpleAND()
        {
            var target = ExpressionToQueryConverter.Convert(
                d => d["age"] > 10 && d["age"] < 20
                );

            var q = Query.And(
                Query.GT("age", 10),
                Query.LT("age", 20)
                );

            target
                .ToString()
                .Should().Be(q.ToString());
        }

        [Test]
        public void GTE()
        {
            var target = ExpressionToQueryConverter.Convert(
                d => d["age"] >= 10
                );

            var q = Query.GTE("age", 10);

            target
                .ToString()
                .Should().Be(q.ToString());
        }

        [Test]
        public void LTE()
        {
            var target = ExpressionToQueryConverter.Convert(
                d => d["age"] <= 10
                );

            var q = Query.LTE("age", 10);

            target
                .ToString()
                .Should().Be(q.ToString());
        }

    }
}