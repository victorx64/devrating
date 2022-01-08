namespace devrating.git.fake;

public sealed class DelayedPatches : Patches
{
    private readonly Patches _origin;
    private readonly TimeSpan _delay;

    public DelayedPatches(Patches origin, TimeSpan delay)
    {
        _origin = origin;
        _delay = delay;
    }

    public IEnumerable<Deletions> Items()
    {
        Task.Delay(_delay).ConfigureAwait(false).GetAwaiter().GetResult();

        return _origin.Items();
    }
}
