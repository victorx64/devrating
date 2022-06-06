using devrating.git.fake;
using Xunit;

namespace devrating.git.test;

public sealed class GitBlameTest
{
    [Fact]
    public void ContainsLine()
    {
        Assert.True(new GitBlame("email", 1, 1, new FakeDiffSizes(2), "SHA").ContainsLine(1));
    }

    [Fact]
    public void DoesNotContainLineBefore()
    {
        Assert.False(new GitBlame("email", 1, 1, new FakeDiffSizes(2), "SHA").ContainsLine(0));
    }

    [Fact]
    public void DoesNotContainLineAfter()
    {
        Assert.False(new GitBlame("email", 1, 1, new FakeDiffSizes(2), "SHA").ContainsLine(2));
    }

    [Fact]
    public void ReturnsAccountableDeletion()
    {
        Assert.NotEqual(0d, new GitBlame("email", 1, 1, new FakeDiffSizes(2), "SHA").SubDeletion(0, 100).Weight());
    }

    [Fact]
    public void ReturnsAllLinesOnBigDeletionRequest()
    {
        Assert.Equal(10u, new GitBlame("email", 1, 10, new FakeDiffSizes(2), "SHA").SubDeletion(0, 100).Size());
    }
}
