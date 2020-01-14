using Xunit;

namespace DevRating.Domain.Test
{
    public sealed class DefaultDeletionTest
    {
        [Fact]
        public void ReturnsEmailFromCtor()
        {
            var email = "some email";

            Assert.Equal(new DefaultDeletion(email, 1).Email(), email);
        }

        [Fact]
        public void ReturnsCountFromCtor()
        {
            var count = 2u;

            Assert.Equal(new DefaultDeletion("some other email", count).Count(), count);
        }
    }
}