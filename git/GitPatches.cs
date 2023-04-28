using Microsoft.Extensions.Logging;

namespace devrating.git;

public sealed class GitPatches : Patches
{
    private readonly string _base;
    private readonly string _commit;
    private readonly string? _since;
    private readonly string _repository;
    private readonly ILoggerFactory _log;
    private readonly DiffSizes _sizes;
    private readonly IEnumerable<string> _paths;

    public GitPatches(
        ILoggerFactory log,
        string @base,
        string commit,
        string? since,
        string repository,
        DiffSizes sizes,
        IEnumerable<string> paths)
    {
        _log = log;
        _commit = commit;
        _base = @base;
        _repository = repository;
        _since = since;
        _sizes = sizes;
        _paths = paths;
    }

    public IEnumerable<Deletions> Items()
    {
        return Task.WhenAll(ItemTasks())
            .GetAwaiter()
            .GetResult();
    }

    private enum State
    {
        Diff,
        OldPath
    }

    private IEnumerable<Task<Deletions>> ItemTasks()
    {
        var patch = new List<string>();
        var old = "unknown";
        var state = State.Diff;
        var args = new List<string> {
            "diff",
            $"{_base}..{_commit}",
            "-U0",
            Config.GitDiffWhitespace,
            Config.GitDiffRenames,
            "--",
        };

        args.AddRange(_paths);

        foreach (var line in new GitProcess(
            _log,
            "git",
            args,
            _repository).Output())
        {
            switch (state)
            {
                case State.Diff:
                    if (line.StartsWith("--- ", StringComparison.Ordinal))
                    {
                        old = "." + line.Substring(5).TrimEnd();
                        state = State.OldPath;
                    }

                    break;
                case State.OldPath:
                    if (line.StartsWith("diff --git ", StringComparison.Ordinal))
                    {
                        yield return FilePatchTask(patch.ToArray(), old.ToString());

                        old = "empty";
                        patch.Clear();
                        state = State.Diff;
                    }

                    break;
            }

            patch.Add(line);
        }

        yield return FilePatchTask(patch.ToArray(), old.ToString());
    }

    private Task<Deletions> FilePatchTask(IEnumerable<string> patch, string old)
    {
        return Task.Run(
            () => (Deletions)new GitDeletions(
                patch,
                new GitAFileBlames(
                    _log,
                    _repository,
                    old,
                    _base,
                    _since,
                    _sizes
                )
            )
        );
    }
}
