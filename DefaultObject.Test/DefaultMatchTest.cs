using Xunit;

namespace DevRating.DefaultObject.Test
{
    public sealed class DefaultMatchTest
    {
        [Fact]
        public void ReturnsContenderRatingFromCtor()
        {
            var rating = 1d;

            Assert.Equal(rating, new DefaultMatch(rating, 1).ContenderRating());
        }

        [Fact]
        public void ReturnsCountFromCtor()
        {
            var count = 2u;

            Assert.Equal(count, new DefaultMatch(2d, count).Count());
        }
    }
}