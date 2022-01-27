using devrating.git.fake;
using Xunit;

namespace devrating.git.test;

public sealed class CountedBlameTest
{
    [Fact]
    public void ContainsLine()
    {
        Assert.True(new GitBlame("email", 1, 1, true, new FakeDiffSizes(2), string.Empty).ContainsLine(1));
    }

    [Fact]
    public void DoesNotContainLineBefore()
    {
        Assert.False(new GitBlame("email", 1, 1, true, new FakeDiffSizes(2), string.Empty).ContainsLine(0));
    }

    [Fact]
    public void DoesNotContainLineAfter()
    {
        Assert.False(new GitBlame("email", 1, 1, true, new FakeDiffSizes(2), string.Empty).ContainsLine(2));
    }

    [Fact]
    public void ReturnsAccountableDeletion()
    {
        Assert.True(new GitBlame("email", 1, 1, true, new FakeDiffSizes(2), string.Empty).SubDeletion(0, 100).DeletionAccountable());
    }

    [Fact]
    public void ReturnsNotAccountableDeletion()
    {
        Assert.False(new GitBlame("email", 1, 1, false, new FakeDiffSizes(2), string.Empty).SubDeletion(0, 100).DeletionAccountable());
    }

    [Fact]
    public void ReturnsAllLinesOnBigDeletionRequest()
    {
        Assert.Equal(10u, new GitBlame("email", 1, 10, true, new FakeDiffSizes(2), string.Empty).SubDeletion(0, 100).DeletedLines());
    }
}
