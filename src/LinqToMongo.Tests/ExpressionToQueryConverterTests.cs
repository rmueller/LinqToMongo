using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using MongoDB.Driver.Builders;
using SharpTestsEx;

namespace LinqToMongo.Tests
{
    [TestFixture]
    public class ExpressionToQueryConverterTests
    {
        [Test]
        public void SimpleEQ()
        {
            var target = ExpressionToQueryConverter.Convert(d => d["name"] == "John");
            var q = Query.EQ("name", "John");
            target
                .ToString()
                .Should().Be(q.ToString());
        }
    }
}
