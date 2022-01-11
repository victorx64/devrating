using devrating.factory;

namespace devrating.git;

public sealed class GitContemporaryLines : ContemporaryLines
{
    private readonly uint _allLines;
    private readonly uint _deletedLines;
    private readonly bool _deletionAccountable;
    private readonly string _victimEmail;

    public GitContemporaryLines(
        uint allLines,
        uint deletedLines,
        bool deletionAccountable,
        string victimEmail
    )
    {
        _allLines = allLines;
        _deletedLines = deletedLines;
        _deletionAccountable = deletionAccountable;
        _victimEmail = victimEmail;
    }

    public uint AllLines()
    {
        return _allLines;
    }

    public uint DeletedLines()
    {
        return _deletedLines;
    }

    public bool DeletionAccountable()
    {
        return _deletionAccountable;
    }

    public string VictimEmail()
    {
        return _victimEmail;
    }
}
