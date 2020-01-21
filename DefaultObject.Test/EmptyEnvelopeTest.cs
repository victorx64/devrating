using System;
using Xunit;

namespace DevRating.DefaultObject.Test
{
    public sealed class EmptyEnvelopeTest
    {
        [Fact]
        public void ReturnsNullValue()
        {
            Assert.Equal(DBNull.Value, new EmptyEnvelope().Value());
        }
    }
}