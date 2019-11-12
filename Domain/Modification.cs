namespace DevRating.Domain
{
    public interface Modification
    {
        Commit Commit();
        uint Count();
    }
}