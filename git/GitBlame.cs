using devrating.factory;

namespace devrating.git;

public sealed class GitBlame : Blame
{
    private readonly string _email;
    private readonly uint _count;
    private readonly uint _start;
    private readonly DiffSizes _sizes;
    private readonly string _sha;

    public GitBlame(
        string email,
        uint start,
        uint count,
        DiffSizes sizes,
        string sha
    )
    {
        _email = email;
        _start = start;
        _count = count;
        _sizes = sizes;
        _sha = sha;
    }

    public bool ContainsLine(uint line)
    {
        return _start <= line && line < _start + _count;
    }

    public ContemporaryLines SubDeletion(uint from, uint to)
    {
        from = Math.Min(Math.Max(_start, from), _start + _count);
        to = Math.Max(Math.Min(_start + _count, to), _start);
        var size = to - from;

        return new GitContemporaryLines(
                (double)size / (double)_sizes.Additions(_sha),
            size,
            _email
        );
    }
}
