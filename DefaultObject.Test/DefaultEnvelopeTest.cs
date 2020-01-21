using System;
using Xunit;

namespace DevRating.DefaultObject.Test
{
    public sealed class DefaultEnvelopeTest
    {
        [Fact]
        public void ReturnsValueFromCtor()
        {
            Assert.Equal(123123, new DefaultEnvelope(123123).Value());
        }

        [Fact]
        public void ReturnsNullValueByDefault()
        {
            Assert.Equal(DBNull.Value, new DefaultEnvelope().Value());
        }
    }
}