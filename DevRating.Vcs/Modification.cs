namespace DevRating.Vcs
{
    public interface Modification
    {
        Commit Commit();
        uint Count();
    }
}