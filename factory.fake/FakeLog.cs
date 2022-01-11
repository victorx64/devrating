namespace devrating.factory.fake;

public sealed class FakeLog : Log
{
    private readonly IList<string> _lines;

    public FakeLog() : this(new List<string>())
    {
    }

    public FakeLog(IList<string> lines)
    {
        _lines = lines;
    }

    public void WriteLine()
    {
        _lines.Add(string.Empty);
    }

    public void WriteLine(string value)
    {
        _lines.Add(value);
    }
}
