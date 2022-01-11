using devrating.factory.fake;
using Xunit;

namespace devrating.git.test;

public sealed class GitProcessTest
{
    [Fact]
    public void ReturnsOutputLines()
    {
        Assert.Equal(2, new GitProcess(new FakeLog(), "dotnet", "--version", ".").Output().Count);
    }
}
