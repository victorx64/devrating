using Xunit;

namespace devrating.consoleapp.test;

public sealed class SystemConsoleTest
{
    [Fact]
    public void WritesLine()
    {
        var stream = new StringWriter();

        Console.SetOut(stream);

        var expected = "123";

        new StandardOutput().WriteLine(expected);

        Assert.Equal(expected + Environment.NewLine, stream.ToString());
    }

    [Fact]
    public void WritesEmptyLine()
    {
        var stream = new StringWriter();

        Console.SetOut(stream);

        new StandardOutput().WriteLine();

        Assert.Equal(Environment.NewLine, stream.ToString());
    }
}
