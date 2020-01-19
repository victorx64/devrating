using DevRating.Domain;

namespace DevRating.VersionControl
{
    public interface Hunk
    {
        Deletions Deletions();
        Additions Additions();
    }
}