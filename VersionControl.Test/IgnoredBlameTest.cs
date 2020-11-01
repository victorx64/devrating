using Xunit;

namespace DevRating.VersionControl.Test
{
    public sealed class IgnoredBlameTest
    {
        [Fact]
        public void ContainsLine()
        {
            Assert.True(new IgnoredBlame("email", 1, 1).ContainsLine(1));
        }

        [Fact]
        public void DoesNotContainLineBefore()
        {
            Assert.False(new IgnoredBlame("email", 1, 1).ContainsLine(0));
        }

        [Fact]
        public void DoesNotContainLineAfter()
        {
            Assert.False(new IgnoredBlame("email", 1, 1).ContainsLine(2));
        }

        [Fact]
        public void ReturnsZeroCountedLines()
        {
            Assert.Equal(0u, new IgnoredBlame("email", 1, 1).SubDeletion(0, 100).Counted());
        }

        [Fact]
        public void ReturnsAllIgnoredLinesOnBigDeletionRequest()
        {
            Assert.Equal(10u, new IgnoredBlame("email", 1, 10).SubDeletion(0, 100).Ignored());
        }
    }
}