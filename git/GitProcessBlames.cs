using devrating.factory;

namespace devrating.git;

public sealed class GitProcessBlames : AFileBlames
{
    private IEnumerable<Blame>? _blames;
    private readonly object _lock = new object();
    private readonly Process _git;
    private readonly DiffSizes _sizes;
    private readonly string? _stop;

    public GitProcessBlames(Log log, string path, string filename, string start, string? stop, string branch)
        : this(
            new GitProcess(log, "git", $"blame -t -e -l -w {(stop is object ? stop + ".." : "")}{start} -- \"{filename}\"", path),
            new GitProcessDiffSizes(log, path, branch),
            stop
        )
    {
    }

    public GitProcessBlames(Process git, DiffSizes sizes, string? stop)
    {
        _git = git;
        _sizes = sizes;
        _stop = stop;
    }

    public Blame AtLine(uint line)
    {
        bool predicate(Blame b)
        {
            return b.ContainsLine(line);
        }

        return InitedBlames().First(predicate);
    }

    private IEnumerable<Blame> InitedBlames()
    {
        if (_blames == null)
        {
            lock (_lock)
            {
                if (_blames == null)
                {
                    var output = _git.Output();

                    _blames = BlameHunks(output, _stop).ToArray();
                }
            }
        }

        return _blames;
    }

    private IEnumerable<Blame> BlameHunks(IList<string> lines, string? stop)
    {
        var current = lines[0];
        var accum = 1u;
        var since = "^" + (stop?.Substring(0, stop.Length - 1) ?? "%%%");

        for (var i = 1; i < lines.Count - 1; i++)
        {
            var line = lines[i];

            if (!EqualShas(line, current))
            {
                if (
                    current.StartsWith('^') &&
                    !current.StartsWith(since, StringComparison.Ordinal)
                )
                {
                    throw new InvalidOperationException(
                        "git log is not deep enough to know whose line is deleted"
                    );
                }

                yield return new GitBlame(
                    Email(current),
                    (uint)i - accum,
                    accum,
                    !current.StartsWith(since, StringComparison.OrdinalIgnoreCase),
                    _sizes.Additions(current)
                );
                current = line;
                accum = 1u;
            }
            else
            {
                accum++;
            }
        }

        if (lines.Count > 1)
        {
            var i = lines.Count - 1;

            yield return new GitBlame(
                Email(current),
                (uint)i - accum,
                accum,
                !current.StartsWith(since, StringComparison.OrdinalIgnoreCase),
                _sizes.Additions(current)
            );
        }
    }

    private bool EqualShas(string a, string b)
    {
        return a.Substring(0, 40).Equals(b.Substring(0, 40), StringComparison.OrdinalIgnoreCase);
    }

    private string Email(string line)
    {
        var start = line.IndexOf('<');
        return line.Substring(start + 1, line.IndexOf('>') - start - 1);
    }
}
