namespace devrating.consoleapp.fake;

public sealed class FakeOutput : Output
{
    private readonly IList<string> _lines;

    public FakeOutput(IList<string> lines)
    {
        _lines = lines;
    }

    public void WriteLine()
    {
        WriteLine(string.Empty);
    }

    public void WriteLine(string value)
    {
        _lines.Add(value);
    }
}
