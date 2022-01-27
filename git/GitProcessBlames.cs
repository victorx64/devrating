using Microsoft.Extensions.Logging;

namespace devrating.git;

public sealed class GitProcessBlames : AFileBlames
{
    private IEnumerable<Blame>? _blames;
    private readonly object _lock = new object();
    private readonly Process _blame;
    private readonly DiffSizes _sizes;

    public GitProcessBlames(ILoggerFactory log, string path, string filename, string start, string? stop, string branch)
        : this(
            new GitProcess(log, "git", $"blame -t -e -l -w {(stop is object ? stop + ".." : "")}{start} -- \"{filename}\"", path),
            new GitProcessDiffSizes(log, path, branch)
        )
    {
    }

    public GitProcessBlames(Process blame, DiffSizes sizes)
    {
        _blame = blame;
        _sizes = sizes;
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
                    var output = _blame.Output();

                    _blames = BlameHunks(output).ToArray();
                }
            }
        }

        return _blames;
    }

    private IEnumerable<Blame> BlameHunks(IList<string> lines)
    {
        var current = lines[0];
        var accum = 1u;

        for (var i = 1; i < lines.Count - 1; i++)
        {
            var line = lines[i];

            if (!EqualShas(line, current))
            {
                yield return new GitBlame(
                    Email(current),
                    (uint)i - accum,
                    accum,
                    !current.StartsWith('^'),
                    _sizes,
                    current.Substring(0, 40)
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
                !current.StartsWith('^'),
                _sizes,
                current.Substring(0, 40)
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
