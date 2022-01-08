using devrating.factory;

namespace devrating.git.fake;

public sealed class FakeDeletions : Deletions
{
    private readonly IEnumerable<ContemporaryLines> _items;

    public FakeDeletions(IEnumerable<ContemporaryLines> items)
    {
        _items = items;
    }

    public IEnumerable<ContemporaryLines> Items()
    {
        return _items;
    }
}
