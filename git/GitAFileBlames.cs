using Microsoft.Extensions.Logging;

namespace devrating.git;

public sealed class GitAFileBlames : AFileBlames
{
    private IEnumerable<Blame>? _blames;
    private readonly object _lock = new object();
    private readonly Process _blame;
    private readonly DiffSizes _sizes;
    private readonly bool _accountable;

    public GitAFileBlames(
        ILoggerFactory log,
        string repository,
        string filename,
        string revision,
        string? since,
        DiffSizes sizes
    ) : this(
            new GitProcess(
                log,
                "git",
                new[]
                {
                    "blame",
                    "-t",
                    "-e",
                    "-l",
                    "--first-parent",
                    Config.GitDiffWhitespace,
                    Config.GitDiffRenames,
                    since is object ? since + ".." + revision : revision,
                    "--",
                    filename,
                },
                repository
            ),
            sizes,
            !revision.Equals(since, StringComparison.Ordinal)
        )
    {
        if (revision.EndsWith('~'))
        {
            throw new InvalidOperationException("Use absolute revision");
        }
    }

    public GitAFileBlames(Process blame, DiffSizes sizes, bool accountable)
    {
        _blame = blame;
        _sizes = sizes;
        _accountable = accountable;
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
                var accountable = _accountable && !current.StartsWith('^');

                if (accountable)
                {
                    yield return new GitBlame(
                        Email(current),
                        (uint)i - accum,
                        accum,
                        _sizes,
                        Sha(current)
                    );
                }
                else
                {
                    yield return new NotCountableBlame(
                        (uint)i - accum,
                        accum
                    );
                }

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

            var accountable = _accountable && !current.StartsWith('^');

            if (accountable)
            {
                yield return new GitBlame(
                    Email(current),
                    (uint)i - accum,
                    accum,
                    _sizes,
                    Sha(current)
                );
            }
            else
            {
                yield return new NotCountableBlame(
                    (uint)i - accum,
                    accum
                );
            }
        }
    }

    private string Email(string line)
    {
        var start = line.IndexOf('<');
        return line.Substring(start + 1, line.IndexOf('>') - start - 1);
    }

    private string Sha(string line)
    {
        return line.Substring(0, 40);
    }

    private bool EqualShas(string a, string b)
    {
        return Sha(a).Equals(Sha(b), StringComparison.OrdinalIgnoreCase);
    }
}
