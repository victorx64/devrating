namespace DevRating.Git
{
    public interface Modification
    {
        Commit Commit();
        int Count();
    }
}