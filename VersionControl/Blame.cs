using DevRating.Domain;

namespace DevRating.VersionControl
{
    public interface Blame
    {
        bool ContainsLine(uint line);
        Deletion Deletion(uint i, uint limit);
    }
}