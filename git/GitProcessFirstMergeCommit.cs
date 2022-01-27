using Microsoft.Extensions.Logging;

namespace devrating.git;

public sealed class GitProcessFirstMergeCommit : GitObject
{
    private readonly Process _after;
    private readonly Process _firstParentRevs;
    private readonly Process _ancestryPathRevs;

    public GitProcessFirstMergeCommit(ILoggerFactory log, string repository, string after, string destination)
        : this(
            new GitProcess(log, "git", $"rev-parse {after}", repository),
            new GitProcess(log, "git", $"rev-list {destination} ^{after.TrimStart('^')} --first-parent", repository),
            new GitProcess(log, "git", $"rev-list {destination} ^{after.TrimStart('^')} --ancestry-path", repository)
        )
    {
    }

    public GitProcessFirstMergeCommit(Process after, Process firstParentRevs, Process ancestryPathRevs)
    {
        _after = after;
        _firstParentRevs = firstParentRevs;
        _ancestryPathRevs = ancestryPathRevs;
    }

    public string Sha()
    {
        var after = _after.Output()[0].TrimEnd();
        var fpRevs = _firstParentRevs.Output();
        var apRevs = _ancestryPathRevs.Output();
        var common = after;

        for (var i = 0; i < apRevs.Count; i++)
        {
            if (
                fpRevs.Count <= i ||
                !fpRevs[i].TrimEnd()
                    .Equals(apRevs[i].TrimEnd(), StringComparison.Ordinal)
            )
            {
                return common;
            }

            common = apRevs[i].TrimEnd();
        }

        return after;
    }
}