using DevRating.Domain.Fake;
using Xunit;

namespace DevRating.Domain.Test
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

            Assert.Equal(new TotalAdditions(hunks).Count(), 10u + 11u + 12u);
        }
    }
}