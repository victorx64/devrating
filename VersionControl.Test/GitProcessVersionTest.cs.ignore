using Semver;
using Xunit;

namespace DevRating.VersionControl.Test
{
    public sealed class GitProcessVersionTest
    {
        [Fact]
        public void ParsesCorrectly()
        {
            Assert.Equal(
                new SemVersion(2, 22, 0),
                new GitProcessVersion("git version 2.22.0.windows.1").Version()
            );
        }
    }
}