using Semver;

namespace devrating.git;

public sealed class LastMajorUpdateTag : Tag
{
    private readonly Tag? _release;

    public LastMajorUpdateTag(IEnumerable<Tag> releases)
        : this(
            releases.Where(r => r.Version() is object)
                .OrderByDescending(r => r.Version())
                .ToList()
        )
    {
    }

    private LastMajorUpdateTag(IList<Tag> releases)
        : this(
            releases.Any() && releases.First().Version()!.Major != releases.Last().Version()!.Major
                ? releases.Last(r => r.Version()!.Major == releases.First().Version()!.Major)
                : null
        )
    {
    }

    private LastMajorUpdateTag(Tag? release)
    {
        _release = release;
    }

    public string? Sha()
    {
        return _release?.Sha();
    }

    public SemVersion? Version()
    {
        return _release?.Version();
    }
}
