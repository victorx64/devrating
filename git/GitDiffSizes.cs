using Microsoft.Extensions.Logging;

namespace devrating.git;

public sealed class GitDiffSizes : DiffSizes
{
    private readonly Dictionary<string, uint> _additions = new Dictionary<string, uint>();
    private readonly object _lock = new object();
    private readonly string _repository;
    private readonly ILoggerFactory _log;

    public GitDiffSizes(ILoggerFactory log, string repository)
    {
        _repository = repository;
        _log = log;
    }

    public uint Additions(string sha)
    {
        if (!_additions.ContainsKey(sha))
        {
            lock (_lock)
            {
                if (!_additions.ContainsKey(sha))
                {
                    if (sha.StartsWith('^'))
                    {
                        var log = _log.CreateLogger(this.GetType());

                        log.LogError(new EventId(1990949), $"We've encountered boundary commit `{sha}`. " +
                            "Try to clone the repository with deeper history " +
                            "(do `git clone` with higher `--depth` argument).");

                        throw new InvalidOperationException($"Encountered boundary commit {sha}");
                    }
                    else
                    {
                        var stat = new GitProcess(
                            _log,
                            "git",
                            $"diff {sha}~..{sha} -U0 {Config.GitDiffArguments} --shortstat",
                            _repository
                        ).Output()[0];

                        var start = stat.IndexOf("changed, ") + "changed, ".Length;
                        var end = stat.IndexOf(" insert");

                        _additions[sha] = uint.Parse(stat.Substring(start, end - start));
                    }
                }
            }
        }

        return _additions[sha];
    }
}
