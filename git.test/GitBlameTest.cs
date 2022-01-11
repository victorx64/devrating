using Xunit;

namespace devrating.git.test;

public sealed class CountedBlameTest
{
    [Fact]
    public void ContainsLine()
    {
        Assert.True(new GitBlame("email", 1, 1, true, 2).ContainsLine(1));
    }

    [Fact]
    public void DoesNotContainLineBefore()
    {
        Assert.False(new GitBlame("email", 1, 1, true, 2).ContainsLine(0));
    }

    [Fact]
    public void DoesNotContainLineAfter()
    {
        Assert.False(new GitBlame("email", 1, 1, true, 2).ContainsLine(2));
    }

    [Fact]
    public void ReturnsAccountableDeletion()
    {
        Assert.True(new GitBlame("email", 1, 1, true, 2).SubDeletion(0, 100).DeletionAccountable());
    }

    [Fact]
    public void ReturnsNotAccountableDeletion()
    {
        Assert.False(new GitBlame("email", 1, 1, false, 2).SubDeletion(0, 100).DeletionAccountable());
    }

    [Fact]
    public void ReturnsAllLinesOnBigDeletionRequest()
    {
        Assert.Equal(10u, new GitBlame("email", 1, 10, true, 2).SubDeletion(0, 100).DeletedLines());
    }
}
