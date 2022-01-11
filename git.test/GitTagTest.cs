using Semver;
using Xunit;

namespace devrating.git.test;

public sealed class GitTagTest
{
    [Fact]
    public void ReturnsSha()
    {
        var expected = "sha";

        Assert.Equal(expected, new GitTag(expected, string.Empty).Sha());
    }

    [Fact]
    public void ReturnsEnvelopedSha()
    {
        var expected = "sha";

        Assert.Equal(expected, new GitTag(expected, null as SemVersion).Sha());
    }

    [Fact]
    public void ReturnsInjectedVersion()
    {
        var expected = new SemVersion(3, 4, 5);

        Assert.Equal(expected, new GitTag("sha", expected).Version());
    }

    [Fact]
    public void HasVersionWhenInjected()
    {
        Assert.True(new GitTag("sha", new SemVersion(3, 4, 5)).Version() is object);
    }

    [Fact]
    public void ParsesVersionWithV()
    {
        Assert.True(new GitTag("sha", "v1.0.0").Version() is object);
    }

    [Fact]
    public void ParsesVersionWithCapitalV()
    {
        Assert.True(new GitTag("sha", "V1.0.0").Version() is object);
    }

    [Fact]
    public void ParsesVersionWithoutV()
    {
        Assert.True(new GitTag("sha", "1.0.0").Version() is object);
    }

    [Fact]
    public void DoesNotParseNameWithOtherPrefix()
    {
        Assert.False(new GitTag("sha", "_1.0.0").Version() is object);
    }

    [Fact]
    public void DoesNotParseNameWithoutVersion()
    {
        Assert.False(new GitTag("sha", "random-text").Version() is object);
    }

    [Fact]
    public void ReturnsParsedVersion()
    {
        Assert.Equal(new SemVersion(2, 3, 5), new GitTag("sha", "2.3.5").Version());
    }
}
