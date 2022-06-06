using devrating.factory;

namespace devrating.git;

public sealed class NotCountableBlame : Blame
{
    private readonly uint _count;
    private readonly uint _start;

    public NotCountableBlame(
        uint start,
        uint count
    )
    {
        _start = start;
        _count = count;
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

        return new NotCountableContemporaryLines(size);
    }
}
