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

        [Fact]
        public void IsEqualToOtherIdWithTheSameValue()
        {
            Assert.True(new DefaultId("the same text").Equals(new DefaultId("the same text")));
        }

        [Fact]
        public void IsNotEqualToOtherIdWithOtherValue()
        {
            Assert.False(new DefaultId("text").Equals(new DefaultId("not the same text")));
        }

        [Fact]
        public void ImplementsEquityOperator()
        {
            Assert.True(new DefaultId("the same text") == (new DefaultId("the same text")));
        }

        [Fact]
        public void ImplementsNotEquityOperator()
        {
            Assert.True(new DefaultId("text") != new DefaultId("not the same text"));
        }

        [Fact]
        public void ReturnsTheSameHashForSameValues()
        {
            Assert.Equal(new DefaultId("the same text").GetHashCode(), new DefaultId("the same text").GetHashCode());
        }

        [Fact]
        public void ChecksEquityWithObject()
        {
            // ReSharper disable once SuspiciousTypeConversion.Global
            Assert.False(new DefaultId("the same text").Equals(Guid.NewGuid()));
        }
    }
}