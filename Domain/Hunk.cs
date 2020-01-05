namespace DevRating.Domain
{
    public interface Hunk
    {
        Deletions Deletions();
        Additions Additions();
    }
}