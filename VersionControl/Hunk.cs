namespace DevRating.VersionControl
{
    public interface Hunk
    {
        Deletions Deletions();
        Additions Additions();
    }
}