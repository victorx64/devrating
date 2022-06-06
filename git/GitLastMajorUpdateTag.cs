using Microsoft.Extensions.Logging;
using Semver;

namespace devrating.git;

public sealed class GitLastMajorUpdateTag : Tag
{
    private readonly Tag _release;

    public GitLastMajorUpdateTag(ILoggerFactory loggerFactory, string repository, string before)
        : this(
            loggerFactory,
            before,
            new GitProcess(
                loggerFactory,
                "git", $"tag -l --format=\"%(objectname) %(refname:short)\" --merged {before}",
                repository
            )
                .Output()
                .Where(l => !string.IsNullOrEmpty(l))
                .Select(t => new GitTag(t.Split(' ')[0], t.Split(' ')[1]))
        )
    {
    }

    private GitLastMajorUpdateTag(ILoggerFactory loggerFactory, string before, IEnumerable<GitTag> releases)
    {
        _release = new LastMajorUpdateTag(releases);

        loggerFactory.CreateLogger<GitLastMajorUpdateTag>().LogInformation(
            new EventId(1890587),
            $"Last major update before {before} (including it) is `{_release.Version()}` (`{_release.Sha()}`)"
        );
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
