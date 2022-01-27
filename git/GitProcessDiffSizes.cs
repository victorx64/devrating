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
                    var merge = new GitProcessFirstMergeCommit(_log, _repository, sha, _branch).Sha();

                    // Keep git arguments in sync with GitProcessPatches and GitProcessBlames
                    var stat = new GitProcess(
                        _log,
                        "git",
                        $"diff {merge}~..{merge} -U0 -M01 -w --shortstat",
                        _repository
                    ).Output()[0];

                    var start = stat.IndexOf("changed, ") + "changed, ".Length;
                    var end = stat.IndexOf(" insert");

                    var insertions = uint.Parse(stat.Substring(start, end - start));

                    _additions[sha] = insertions;
                }
            }
        }

        return _additions[sha];
    }
}
