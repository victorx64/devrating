using devrating.factory;

namespace devrating.git;

public sealed class GitBlame : Blame
{
    private readonly string _email;
    private readonly uint _count;
    private readonly uint _start;
    private readonly bool _accountable;
    private readonly DiffSizes _sizes;
    private readonly string _sha;

    public GitBlame(
        string email,
        uint start,
        uint count,
        bool accountable,
        DiffSizes sizes,
        string sha
    )
    {
        _email = email;
        _start = start;
        _count = count;
        _accountable = accountable;
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
        
        return new GitContemporaryLines(
            _sizes.Additions(_sha),
            to - from,
            _accountable,
            _email
        );
    }
}
