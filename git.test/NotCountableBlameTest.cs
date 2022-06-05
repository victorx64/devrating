using Xunit;

namespace devrating.git.test;

public sealed class NotCountableBlameTest
{
    [Fact]
    public void ContainsLine()
    {
        Assert.True(new NotCountableBlame(1, 1).ContainsLine(1));
    }

    [Fact]
    public void DoesNotContainLineBefore()
    {
        Assert.False(new NotCountableBlame(1, 1).ContainsLine(0));
    }

    [Fact]
    public void DoesNotContainLineAfter()
    {
        Assert.False(new NotCountableBlame(1, 1).ContainsLine(2));
    }

    [Fact]
    public void ReturnsNotAccountableDeletion()
    {
        Assert.Equal(0d, new NotCountableBlame(1, 1).SubDeletion(0, 100).Weight());
    }

    [Fact]
    public void ReturnsAllLinesOnBigDeletionRequest()
    {
        Assert.Equal(10u, new NotCountableBlame(1, 10).SubDeletion(0, 100).Size());
    }
}
