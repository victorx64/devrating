using devrating.git.fake;
using Xunit;

namespace devrating.git.test;

public sealed class GitProcessBlamesTest
{
    [Fact]
    public void CombinesLinesWithSameRevision()
    {
        var collection = new GitProcessBlames(
            new FakeProcess(
@"^7f30cd2 (<svikk@live.ru>              1595608723 +0900  1) sv
^7f30cd2 (<svikk@live.ru>              1595608723 +0900  2) sv
661ab997 (<viktor_semenov@outlook.com> 1603634378 +0900  3) sv1
661ab997 (<viktor_semenov@outlook.com> 1603634378 +0900  4) sv1
661ab997 (<viktor_semenov@outlook.com> 1603634378 +0900  5) sv11
"
            ),
            new FakeDiffSizes(1u),
            "7f30cd25"
        );

        Assert.Equal(
            3u,
            collection.AtLine(2).SubDeletion(0, 1000).DeletedLines()
        );
    }

    [Fact]
    public void ThrowsIfEndOfLogIsReached()
    {
        Assert.Throws<System.InvalidOperationException>(
            () =>
            new GitProcessBlames(
                new FakeProcess(
@"^7f30cd2 (<svikk@live.ru>              1595608723 +0900  1) sv
^7f30cd2 (<svikk@live.ru>              1595608723 +0900  2) sv
661ab997 (<viktor_semenov@outlook.com> 1603634378 +0900  3) sv1
661ab997 (<viktor_semenov@outlook.com> 1603634378 +0900  4) sv1
661ab997 (<viktor_semenov@outlook.com> 1603634378 +0900  5) sv11
"
                ),
                new FakeDiffSizes(1u),
                null
            )
            .AtLine(0)
        );
    }

    [Fact]
    public void IgnoresLinesThatStartWithSpecialChar()
    {
        Assert.False(
            new GitProcessBlames(
                new FakeProcess(
@"^7f30cd2 (<svikk@live.ru>              1595608723 +0900  1) sv
^7f30cd2 (<svikk@live.ru>              1595608723 +0900  2) sv
661ab997 (<viktor_semenov@outlook.com> 1603634378 +0900  3) sv1
661ab997 (<viktor_semenov@outlook.com> 1603634378 +0900  4) sv1
661ab997 (<viktor_semenov@outlook.com> 1603634378 +0900  5) sv11
"
                ),
                new FakeDiffSizes(1u),
                "7f30cd25"
            )
            .AtLine(0)
            .SubDeletion(0, 1000)
            .DeletionAccountable()
        );
    }

    [Fact]
    public void CountsLinesThatStartWithAlphanumericalChar()
    {
        Assert.True(
            new GitProcessBlames(
                new FakeProcess(
@"^7f30cd2 (<svikk@live.ru>              1595608723 +0900  1) sv
^7f30cd2 (<svikk@live.ru>              1595608723 +0900  2) sv
661ab997 (<viktor_semenov@outlook.com> 1603634378 +0900  3) sv1
661ab997 (<viktor_semenov@outlook.com> 1603634378 +0900  4) sv1
661ab997 (<viktor_semenov@outlook.com> 1603634378 +0900  5) sv11
"
                ),
                new FakeDiffSizes(1u),
                "7f30cd25"
            )
            .AtLine(2)
            .SubDeletion(0, 1000)
            .DeletionAccountable()
        );
    }

    [Fact]
    public void HandlesSingleLine()
    {
        Assert.Equal(
            "viktor_semenov@outlook.com",
            new GitProcessBlames(
                new FakeProcess(
@"661ab997 (<viktor_semenov@outlook.com> 1603634378 +0900  1) sv1
"
                ),
                new FakeDiffSizes(1u),
                null
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
            new GitProcessBlames(
                new FakeProcess("661ab997 (<viktor_semenov@outlook.com> 1603634378 +0900  1) sv1"),
                new FakeDiffSizes(1u),
                null
            )
            .AtLine(0)
        );
    }

    [Fact]
    public void ParsesEmail()
    {
        Assert.Equal(
            "viktor_semenov@outlook.com",
            new GitProcessBlames(
                new FakeProcess(
@"661ab997 (<viktor_semenov@outlook.com> 1603634378 +0900  1) sv1
"
                ),
                new FakeDiffSizes(1u),
                null
            )
            .AtLine(0)
            .SubDeletion(0, 1000)
            .VictimEmail()
        );
    }
}
