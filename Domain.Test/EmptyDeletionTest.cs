using Xunit;

namespace DevRating.Domain.Test
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