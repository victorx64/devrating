namespace DevRating.Git
{
    public interface Modification
    {
        Author Author();
        Commit Commit();
        int Count();
    }
}