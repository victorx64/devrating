using DevRating.VersionControl.Fake;
using Xunit;

namespace DevRating.VersionControl.Test
{
    public sealed class VersionControlHunkTest
    {
        [Fact]
        public void ReturnsDeletions()
        {
            Assert.NotNull(
                new VersionControlHunk(
                    "patch", 
                    new FakeBlames(
                        new Blame[] { }
                    )
                )
                .Deletions()
            );
        }

        [Fact]
        public void ReturnsAdditions()
        {
            Assert.NotNull(
                new VersionControlHunk(
                    "patch", 
                    new FakeBlames(
                        new Blame[] { }
                    )
                )
                .Additions()
            );
        }
    }
}