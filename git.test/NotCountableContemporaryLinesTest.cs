using Xunit;

namespace devrating.git.test;

public sealed class NotCountableContemporaryLinesTest
{
    [Fact]
    public void ReturnsUnknownEmail()
    {
        Assert.Equal("unknown-e232aac2", new NotCountableContemporaryLines(1).VictimEmail());
    }

    [Fact]
    public void ReturnsDeletedNumberFromCtor()
    {
        var count = 2u;

        Assert.Equal(count, new NotCountableContemporaryLines(count).Size());
    }

    [Fact]
    public void ReturnsZeroWeight()
    {
        Assert.Equal(0d, new NotCountableContemporaryLines(5u).Weight());
    }
}
