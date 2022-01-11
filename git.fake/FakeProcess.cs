namespace devrating.git.fake;

public sealed class FakeProcess : Process
{
    private readonly IList<string> _output;

    public FakeProcess(string output)
        : this(output.Split('\n'))
    {
    }

    public FakeProcess(IList<string> output)
    {
        _output = output;
    }

    public IList<string> Output()
    {
        return _output;
    }
}
