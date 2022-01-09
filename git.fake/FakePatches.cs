namespace devrating.git.fake;

public sealed class FakePatches : Patches
{
    private readonly IEnumerable<Deletions> _items;

    public FakePatches(IEnumerable<Deletions> items)
    {
        _items = items;
    }

    public IEnumerable<Deletions> Items()
    {
        return _items;
    }
}
