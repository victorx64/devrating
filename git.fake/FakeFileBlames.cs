namespace devrating.git.fake;

public sealed class FakeFileBlames : AFileBlames
{
    private readonly IEnumerable<Blame> _blames;

    public FakeFileBlames() : this(new GitBlame[0])
    {
    }

    public FakeFileBlames(IEnumerable<Blame> blames)
    {
        _blames = blames;
    }

    public Blame AtLine(uint line)
    {
        bool predicate(Blame b)
        {
            return b.ContainsLine(line);
        }

        return _blames.Single(predicate);
    }
}
