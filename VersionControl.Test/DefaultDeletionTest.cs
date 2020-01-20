using DevRating.Domain;
using Xunit;

namespace DevRating.VersionControl.Test
{
    public sealed class DefaultDeletionTest
    {
        [Fact]
        public void ReturnsEmailFromCtor()
        {
            var email = "some email";

            Assert.Equal(email, new VersionControlDeletion(email, 1).Email());
        }

        [Fact]
        public void ReturnsCountFromCtor()
        {
            var count = 2u;

            Assert.Equal(count, new VersionControlDeletion("some other email", count).Count());
        }
    }
}