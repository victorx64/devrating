namespace devrating.git;

public sealed class GitProcessPatches : Patches
{
    private readonly string _start;
    private readonly string _end;
    private readonly string? _since;
    private readonly string _repository;
    private readonly string _branch;

    public GitProcessPatches(string start, string end, string? since, string repository, string branch)
    {
        _start = start;
        _end = end;
        _repository = repository;
        _since = since;
        _branch = branch;
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

        foreach (var line in new GitProcess("git", $"diff {_start}..{_end} -U0 -M01 -w", _repository).Output())
        {
            switch (state)
            {
                case State.Diff:
                    if (line.StartsWith("--- ", StringComparison.Ordinal))
                    {
                        old = "." + line.Substring(5);
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
                new GitProcessBlames(
                    _repository,
                    old,
                    _start,
                    _since,
                    _branch
                )
            )
        );
    }
}
