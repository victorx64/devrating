namespace DevRating.Git
{
    public interface ModificationsFactory
    {
        Modifications Modifications(string commit, string author);
    }
}