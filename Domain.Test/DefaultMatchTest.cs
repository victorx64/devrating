using Xunit;

namespace DevRating.Domain.Test
{
    public sealed class DefaultMatchTest
    {
        [Fact]
        public void ReturnsContenderRatingFromCtor()
        {
            var rating = 1d;

            Assert.Equal(new DefaultMatch(rating, 1).ContenderRating(), rating);
        }

        [Fact]
        public void ReturnsCountFromCtor()
        {
            var count = 2u;

            Assert.Equal(new DefaultMatch(2d, count).Count(), count);
        }
    }
}