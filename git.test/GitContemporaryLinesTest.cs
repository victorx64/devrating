using Xunit;

namespace devrating.git.test;

public sealed class GitContemporaryLinesTest
{
    [Fact]
    public void ReturnsEmailFromCtor()
    {
        var email = "some email";

        Assert.Equal(email, new GitContemporaryLines(1, 1, email).VictimEmail());
    }

    [Fact]
    public void ReturnsDeletedNumberFromCtor()
    {
        var count = 2u;

        Assert.Equal(count, new GitContemporaryLines(1, count, "some other email").Size());
    }

    [Fact]
    public void ReturnsWeight()
    {
        var weight = 8.4d;

        Assert.Equal(weight, new GitContemporaryLines(weight, 5u, "another email").Weight());
    }
}
