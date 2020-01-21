using System;
using Xunit;

namespace DevRating.DefaultObject.Test
{
    public sealed class DefaultIdTest
    {
        [Fact]
        public void ReturnsValueFromCtor()
        {
            var id = Guid.NewGuid();

            Assert.Equal(id, new DefaultId(id).Value());
        }

        [Fact]
        public void IsFilledWithValueFromCtor()
        {
            var id = Guid.NewGuid();

            Assert.True(new DefaultId(id).Filled());
        }

        [Fact]
        public void ReturnsDbNullValueByDefault()
        {
            Assert.Equal(DBNull.Value, new DefaultId().Value());
        }

        [Fact]
        public void IsNotFilledByDefault()
        {
            Assert.False(new DefaultId().Filled());
        }
    }
}