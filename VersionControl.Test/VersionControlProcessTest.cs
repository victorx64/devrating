using Xunit;

namespace DevRating.VersionControl.Test
{
    public sealed class VersionControlProcessTest
    {
        [Fact]
        public void ReturnsOutputLines()
        {
            Assert.Equal(2, new VersionControlProcess("dotnet", "--version", ".").Output().Count);
        }
    }
}