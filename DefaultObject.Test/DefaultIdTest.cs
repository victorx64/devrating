using Xunit;

namespace DevRating.DefaultObject.Test
{
    public sealed class DefaultIdTest
    {
        [Fact]
        public void ReturnsValueFromCtor()
        {
            var rating = 1d;

            Assert.Equal(rating, new DefaultId(rating).Value());
        }
    }
}