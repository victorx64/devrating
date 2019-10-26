namespace DevRating.Vcs
{
    public interface Modification
    {
        Commit Commit();
        int Count();
    }
}