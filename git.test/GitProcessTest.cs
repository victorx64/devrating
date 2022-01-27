using Microsoft.Extensions.Logging;
using Xunit;

namespace devrating.git.test;

public sealed class GitProcessTest
{
    [Fact]
    public void ReturnsOutputLines()
    {
        Assert.Equal(2, new GitProcess(new LoggerFactory(), "dotnet", "--version", ".").Output().Count);
    }
}
