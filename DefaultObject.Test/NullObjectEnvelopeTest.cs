using System;
using Xunit;

namespace DevRating.DefaultObject.Test
{
    public sealed class NullObjectEnvelopeTest
    {
        [Fact]
        public void ReturnsNullValue()
        {
            Assert.Equal(DBNull.Value, new NullObjectEnvelope().Value());
        }
    }
}