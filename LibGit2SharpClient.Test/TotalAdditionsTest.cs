using DevRating.VersionControl.Fake;
using Xunit;

namespace DevRating.VersionControl.Test
{
    public sealed class TotalAdditionsTest
    {
        [Fact]
        public void ReturnsTotalSumOfAdditions()
        {
            var hunks = new FakeHunks(new[]
            {
                new FakeHunk(new EmptyDeletions(), new FakeAdditions(10u)),
                new FakeHunk(new EmptyDeletions(), new FakeAdditions(11u)),
                new FakeHunk(new EmptyDeletions(), new FakeAdditions(12u)),
            });

            Assert.Equal(10u + 11u + 12u, new TotalAdditions(hunks).Count());
        }
    }
}