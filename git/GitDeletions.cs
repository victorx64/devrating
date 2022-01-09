using devrating.factory;

namespace devrating.git;

public sealed class GitDeletions : Deletions
{
    private readonly IEnumerable<string> _patch;
    private readonly AFileBlames _blames;

    public GitDeletions(string patch, AFileBlames blames)
        : this(patch.Split('\n'), blames)
    {
    }

    public GitDeletions(IEnumerable<string> patch, AFileBlames blames)
    {
        _patch = patch;
        _blames = blames;
    }

    public IEnumerable<ContemporaryLines> Items()
    {
        var deletions = new List<ContemporaryLines>();

        foreach (var header in HunkHeaders())
        {
            deletions.AddRange(HunkDeletions(header));
        }

        return deletions;
    }

    private IEnumerable<string> HunkHeaders()
    {
        foreach (var line in _patch)
        {
            if (line.StartsWith("@@ "))
            {
                yield return line.Split(' ')[1];
            }
        }
    }

    private IEnumerable<ContemporaryLines> HunkDeletions(string header)
    {
        var parts = HeaderParts(header);
        var index = Index(parts);
        var count = Count(parts);
        uint increment;

        for (var i = index; i < index + count; i += increment)
        {
            var deletion = _blames.AtLine(i).SubDeletion(i, index + count);

            increment = deletion.DeletedLines();

            yield return deletion;
        }
    }

    private IReadOnlyList<string> HeaderParts(string header)
    {
        return header.Substring(1).Split(',');
    }

    private uint Index(IReadOnlyList<string> parts)
    {
        return Convert.ToUInt32(parts[0]) - 1;
    }

    private uint Count(IReadOnlyList<string> parts)
    {
        return parts.Count == 1 ? 1 : Convert.ToUInt32(parts[1]);
    }
}
