namespace devrating.git.fake;

public sealed class FakeDiffSizes : DiffSizes
{
    private readonly uint _additions;

    public FakeDiffSizes(uint additions)
    {
        this._additions = additions;
    }

    public uint Additions(string sha)
    {
        return _additions;
    }
}
