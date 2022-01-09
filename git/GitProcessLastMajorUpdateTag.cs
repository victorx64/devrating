using Semver;

namespace devrating.git;

public sealed class GitProcessLastMajorUpdateTag : Tag
{
    private readonly Tag _release;

    public GitProcessLastMajorUpdateTag(string repository, string before)
        : this(
            new LastMajorUpdateTag(
                new GitProcess("git", $"tag -l --format='%(objectname) %(refname:short)' --merged {before}", repository)
                    .Output()
                    .Where(l => !string.IsNullOrEmpty(l))
                    .Select(t => new GitTag(t.Split(' ')[0], t.Split(' ')[1]))
            )
        )
    {
    }

    private GitProcessLastMajorUpdateTag(Tag release)
    {
        _release = release;
    }

    public string? Sha()
    {
        return _release.Sha();
    }

    public SemVersion? Version()
    {
        return _release.Version();
    }
}
