using System;
using Xunit;

namespace DevRating.DefaultObject.Test
{
    public sealed class NullRatingTest
    {
        [Fact]
        public void ReturnsNullId()
        {
            Assert.Equal(DBNull.Value, new NullRating().Id());
        }

        [Fact]
        public void ReturnsDefaultValue()
        {
            Assert.Equal(24124d, new NullRating(24124d).Value());
        }
    }
}