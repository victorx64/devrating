using Xunit;

namespace devrating.git.test;

public sealed class GitContemporaryLinesTest
{
    [Fact]
    public void ReturnsEmailFromCtor()
    {
        var email = "some email";

        Assert.Equal(email, new GitContemporaryLines(1, 1, true, email).VictimEmail());
    }

    [Fact]
    public void ReturnsDeletedNumberFromCtor()
    {
        var count = 2u;

        Assert.Equal(count, new GitContemporaryLines(1, count, true, "some other email").DeletedLines());
    }

    [Fact]
    public void ReturnsAllLinesNumberFromCtor()
    {
        var count = 3u;

        Assert.Equal(count, new GitContemporaryLines(count, 1, true, "another email").AllLines());
    }
}
