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
    }
}