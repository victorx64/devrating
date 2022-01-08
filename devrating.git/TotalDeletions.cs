using devrating.factory;

namespace devrating.git;

public sealed class TotalDeletions : Deletions
{
    private readonly Patches _patches;

    public TotalDeletions(Patches patches)
    {
        _patches = patches;
    }

    public IEnumerable<ContemporaryLines> Items()
    {
        return _patches.Items().SelectMany(p => p.Items());
    }
}
