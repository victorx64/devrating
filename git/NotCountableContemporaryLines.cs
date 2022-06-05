using devrating.factory;

namespace devrating.git;

public sealed class NotCountableContemporaryLines : ContemporaryLines
{
    private readonly uint _size;

    public NotCountableContemporaryLines(uint size)
    {
        _size = size;
    }

    public double Weight()
    {
        return 0d;
    }

    public string VictimEmail()
    {
        return "unknown-e232aac2";
    }

    public uint Size()
    {
        return _size;
    }
}
