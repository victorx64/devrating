using Semver;
using Xunit;

namespace devrating.git.test;

public sealed class LastMajorUpdateTagTest
{
    [Fact]
    public void HasNoVersionOnEmptyTagsCollection()
    {
        Assert.False(new LastMajorUpdateTag(new Tag[0]).Version() is object);
    }

    [Fact]
    public void HasNoShaOnEmptyTagsCollection()
    {
        Assert.False(new LastMajorUpdateTag(new Tag[0])
            .Sha() is object);
    }

    [Fact]
    public void HasNoVersionOnTagsWithoutVersions()
    {
        Assert.False(
            new LastMajorUpdateTag(
                new[]
                {
                    new GitTag("", "no-version"),
                    new GitTag("", "still-no-version"),
                }
            )
            .Version() is object
        );
    }

    [Fact]
    public void HasNoShaOnTagsWithoutVersions()
    {
        Assert.False(
            new LastMajorUpdateTag(
                new[]
                {
                    new GitTag("", "no-version"),
                    new GitTag("", "still-no-version"),
                }
            )
            .Sha() is object
        );
    }

    [Fact]
    public void HasNoVersionOnSingleTag()
    {
        Assert.False(
            new LastMajorUpdateTag(
                new[]
                {
                    new GitTag("", "0.1.0"),
                }
            )
            .Version() is object
        );
    }

    [Fact]
    public void HasNoShaOnSingleTag()
    {
        Assert.False(
            new LastMajorUpdateTag(
                new[]
                {
                    new GitTag("", "0.1.0"),
                }
            )
            .Sha() is object
        );
    }

    [Fact]
    public void HasNoVersionOnSingleTagWithVersion()
    {
        Assert.False(
            new LastMajorUpdateTag(
                new[]
                {
                    new GitTag("", "no-version"),
                    new GitTag("", "0.1.0"),
                }
            )
            .Version() is object
        );
    }

    [Fact]
    public void HasNoShaOnSingleTagWithVersion()
    {
        Assert.False(
            new LastMajorUpdateTag(
                new[]
                {
                    new GitTag("", "no-version"),
                    new GitTag("", "0.1.0"),
                }
            )
            .Sha() is object
        );
    }

    [Fact]
    public void HasNoVersionOnSingleMajorVersion()
    {
        Assert.False(
            new LastMajorUpdateTag(
                new[]
                {
                    new GitTag("", "10.1.0"),
                    new GitTag("", "10.2.11"),
                    new GitTag("", "10.3.2"),
                    new GitTag("", "10.4.5"),
                }
            )
            .Version() is object
        );
    }

    [Fact]
    public void HasNoShaOnSingleMajorVersion()
    {
        Assert.False(
            new LastMajorUpdateTag(
                new[]
                {
                    new GitTag("", "10.1.0"),
                    new GitTag("", "10.2.11"),
                    new GitTag("", "10.3.2"),
                    new GitTag("", "10.4.5"),
                }
            )
            .Sha() is object
        );
    }

    [Fact]
    public void ReturnsOldestTagVersionWithLatestMajorVersion()
    {
        Assert.Equal(
            new SemVersion(1, 0, 0, "alpha"),
            new LastMajorUpdateTag(
                new[]
                {
                    new GitTag("", "0.1.0"),
                    new GitTag("", "0.1.1"),
                    new GitTag("", "1.0.0-alpha"),
                    new GitTag("", "1.0.0-beta"),
                    new GitTag("", "1.1.0"),
                    new GitTag("", "1.2.0"),
                }
            )
            .Version()
        );
    }

    [Fact]
    public void HasOldestTagVersionWithLatestMajorVersion()
    {
        Assert.True(
            new LastMajorUpdateTag(
                new[]
                {
                    new GitTag("", "0.1.0"),
                    new GitTag("", "0.1.1"),
                    new GitTag("", "1.0.0-alpha"),
                    new GitTag("", "1.0.0-beta"),
                    new GitTag("", "1.1.0"),
                    new GitTag("", "1.2.0"),
                }
            )
            .Version() is object
        );
    }

    [Fact]
    public void ReturnsOldestTagShaWithLatestMajorVersion()
    {
        Assert.Equal(
            "this",
            new LastMajorUpdateTag(
                new[]
                {
                    new GitTag("", "0.1.0"),
                    new GitTag("", "0.1.1"),
                    new GitTag("this", "1.0.0-alpha"),
                    new GitTag("", "1.0.0-beta"),
                    new GitTag("", "1.1.0"),
                    new GitTag("", "1.2.0"),
                }
            )
            .Sha()
        );
    }

    [Fact]
    public void OrdersTagsByVersionBeforeAnalyzing()
    {
        Assert.Equal(
            new SemVersion(1, 0, 0, "alpha"),
            new LastMajorUpdateTag(
                new[]
                {
                    new GitTag("", "1.2.0"),
                    new GitTag("", "0.1.1"),
                    new GitTag("", "1.0.0-beta"),
                    new GitTag("", "1.1.0"),
                    new GitTag("", "0.1.0"),
                    new GitTag("", "1.0.0-alpha"),
                }
            )
            .Version()
        );
    }

    [Fact]
    public void ReturnsSingleTagVersionWithLatestMajorVersion()
    {
        Assert.Equal(
            new SemVersion(2, 0, 0, "latest"),
            new LastMajorUpdateTag(
                new[]
                {
                    new GitTag("", "0.1.0"),
                    new GitTag("", "0.1.1"),
                    new GitTag("", "1.0.0-alpha"),
                    new GitTag("", "1.0.0-beta"),
                    new GitTag("", "1.1.0"),
                    new GitTag("", "1.2.0"),
                    new GitTag("", "2.0.0-latest"),
                }
            )
            .Version()
        );
    }

    [Fact]
    public void HasSingleTagVersionWithLatestMajorVersion()
    {
        Assert.True(
            new LastMajorUpdateTag(
                new[]
                {
                    new GitTag("", "0.1.0"),
                    new GitTag("", "0.1.1"),
                    new GitTag("", "1.0.0-alpha"),
                    new GitTag("", "1.0.0-beta"),
                    new GitTag("", "1.1.0"),
                    new GitTag("", "1.2.0"),
                    new GitTag("", "2.0.0-latest"),
                }
            )
            .Version() is object
        );
    }

    [Fact]
    public void ReturnsSingleTagShaWithLatestMajorVersion()
    {
        Assert.Equal(
            "this",
            new LastMajorUpdateTag(
                new[]
                {
                    new GitTag("", "0.1.0"),
                    new GitTag("", "0.1.1"),
                    new GitTag("", "1.0.0-alpha"),
                    new GitTag("", "1.0.0-beta"),
                    new GitTag("", "1.1.0"),
                    new GitTag("", "1.2.0"),
                    new GitTag("this", "2.0.0-latest"),
                }
            )
            .Sha()
        );
    }
}
