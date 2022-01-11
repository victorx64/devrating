namespace devrating.git;

public sealed class CachedPatches : Patches
{
    private readonly Patches _origin;
    private readonly object _lock = new object();
    private IEnumerable<Deletions>? _items;

    public CachedPatches(Patches origin)
    {
        _origin = origin;
    }

    public IEnumerable<Deletions> Items()
    {
        if (_items is object)
        {
            return _items;
        }

        lock (_lock)
        {
            return _items ??= _origin.Items();
        }
    }
}
