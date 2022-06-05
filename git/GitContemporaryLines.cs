using devrating.factory;

namespace devrating.git;

public sealed class GitContemporaryLines : ContemporaryLines
{
    private readonly double _weight;
    private readonly uint _size;
    private readonly string _victimEmail;

    public GitContemporaryLines(
        double weight,
        uint size,
        string victimEmail
    )
    {
        _weight = weight;
        _size = size;
        _victimEmail = victimEmail;
    }

    public double Weight()
    {
        return _weight;
    }

    public string VictimEmail()
    {
        return _victimEmail;
    }

    public uint Size()
    {
        return _size;
    }
}
