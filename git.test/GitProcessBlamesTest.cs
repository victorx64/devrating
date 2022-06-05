using devrating.git.fake;
using Microsoft.Extensions.Logging;
using Xunit;

namespace devrating.git.test;

public sealed class GitAFileBlamesTest
{
    [Fact]
    public void CombinesLinesWithSameRevision()
    {
        var collection = new GitAFileBlames(
            new LoggerFactory(),
            new FakeProcess(
@"^7f30cd2 (<svikk@live.ru>              1595608723 +0900  1) sv
^7f30cd2 (<svikk@live.ru>              1595608723 +0900  2) sv
661ab997 (<viktor_semenov@outlook.com> 1603634378 +0900  3) sv1
661ab997 (<viktor_semenov@outlook.com> 1603634378 +0900  4) sv1
661ab997 (<viktor_semenov@outlook.com> 1603634378 +0900  5) sv11
"
            ),
            new FakeDiffSizes(1u),
            true
        );

        Assert.Equal(
            3u,
            collection.AtLine(2).SubDeletion(0, 1000).Size()
        );
    }

    [Fact]
    public void IgnoresLinesThatStartWithSpecialChar()
    {
        Assert.Equal(
            0d,
            new GitAFileBlames(
                new LoggerFactory(),
                new FakeProcess(
@"^7f30cd2 (<svikk@live.ru>              1595608723 +0900  1) sv
^7f30cd2 (<svikk@live.ru>              1595608723 +0900  2) sv
661ab997 (<viktor_semenov@outlook.com> 1603634378 +0900  3) sv1
661ab997 (<viktor_semenov@outlook.com> 1603634378 +0900  4) sv1
661ab997 (<viktor_semenov@outlook.com> 1603634378 +0900  5) sv11
"
                ),
                new FakeDiffSizes(1u),
                true
            )
            .AtLine(0)
            .SubDeletion(0, 1000)
            .Weight()
        );
    }

    [Fact]
    public void CountsLinesThatStartWithAlphanumericalChar()
    {
        Assert.NotEqual(
            0d,
            new GitAFileBlames(
                new LoggerFactory(),
                new FakeProcess(
@"^7f30cd2 (<svikk@live.ru>              1595608723 +0900  1) sv
^7f30cd2 (<svikk@live.ru>              1595608723 +0900  2) sv
661ab997 (<viktor_semenov@outlook.com> 1603634378 +0900  3) sv1
661ab997 (<viktor_semenov@outlook.com> 1603634378 +0900  4) sv1
661ab997 (<viktor_semenov@outlook.com> 1603634378 +0900  5) sv11
"
                ),
                new FakeDiffSizes(1u),
                true
            )
            .AtLine(2)
            .SubDeletion(0, 1000)
            .Weight()
        );
    }

    [Fact]
    public void HandlesSingleLine()
    {
        Assert.Equal(
            "viktor_semenov@outlook.com",
            new GitAFileBlames(
                new LoggerFactory(),
                new FakeProcess(
@"661ab997 (<viktor_semenov@outlook.com> 1603634378 +0900  1) sv1
"
                ),
                new FakeDiffSizes(1u),
                true
            )
            .AtLine(0)
            .SubDeletion(0, 1000)
            .VictimEmail()
        );
    }

    [Fact]
    public void ExpectsEmptyLineLast()
    {
        Assert.Throws<System.InvalidOperationException>(
            () =>
            new GitAFileBlames(
                new LoggerFactory(),
                new FakeProcess("661ab997 (<viktor_semenov@outlook.com> 1603634378 +0900  1) sv1"),
                new FakeDiffSizes(1u),
                true
            )
            .AtLine(0)
        );
    }

    [Fact]
    public void ParsesEmail()
    {
        Assert.Equal(
            "viktor_semenov@outlook.com",
            new GitAFileBlames(
                new LoggerFactory(),
                new FakeProcess(
@"661ab997 (<viktor_semenov@outlook.com> 1603634378 +0900  1) sv1
"
                ),
                new FakeDiffSizes(1u),
                true
            )
            .AtLine(0)
            .SubDeletion(0, 1000)
            .VictimEmail()
        );
    }
}
