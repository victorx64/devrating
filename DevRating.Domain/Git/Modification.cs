namespace DevRating.Domain.Git
{
    public interface Modification
    {
        Commit Commit();
        uint Count();
    }
}