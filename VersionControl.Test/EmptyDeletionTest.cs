using Xunit;

namespace DevRating.VersionControl.Test
{
    public sealed class EmptyDeletionTest
    {
        [Fact]
        public void ReturnsEmptyItems()
        {
            Assert.Empty(new EmptyDeletions().Items());
        }
    }
}