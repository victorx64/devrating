using Microsoft.Extensions.Logging;

namespace devrating.git;

public sealed class GitProcessDiffSizes : DiffSizes
{
    private readonly Dictionary<string, uint> _additions = new Dictionary<string, uint>();
    private readonly object _lock = new object();
    private readonly string _repository;
    private readonly string _branch;
    private readonly ILoggerFactory _log;

    public GitProcessDiffSizes(ILoggerFactory log, string repository, string branch)
    {
        _repository = repository;
        _log = log;
        _branch = branch;
    }

    public uint Additions(string sha)
    {
        if (!_additions.ContainsKey(sha))
        {
            lock (_lock)
            {
                if (!_additions.ContainsKey(sha))
                {
                    String stat;

                    if (sha.StartsWith('^'))
                    {
                        var log = _log.CreateLogger(this.GetType());

                        log.LogWarning(new EventId(1990949), $"We've encountered initial commit `{sha}`. " +
                            "If it's not initial commit try to clone the repository with deeper history " +
                            "(do `git clone` with higher `--depth` argument).");

                        // Keep git arguments in sync with GitProcessPatches and GitProcessBlames
                        stat = new GitProcess(
                            _log,
                            "git",
                            $"show {sha.TrimStart('^')} -U0 -M01 -w --shortstat --pretty=oneline",
                            _repository
                        ).Output()[1];
                    }
                    else
                    {
                        var merge = new GitProcessFirstMergeCommit(_log, _repository, sha, _branch).Sha();

                        // Keep git arguments in sync with GitProcessPatches and GitProcessBlames
                        stat = new GitProcess(
                            _log,
                            "git",
                            $"diff {merge}~..{merge} -U0 -M01 -w --shortstat",
                            _repository
                        ).Output()[0];
                    }

                    var start = stat.IndexOf("changed, ") + "changed, ".Length;
                    var end = stat.IndexOf(" insert");

                    _additions[sha] = uint.Parse(stat.Substring(start, end - start));
                }
            }
        }

        return _additions[sha];
    }
}
