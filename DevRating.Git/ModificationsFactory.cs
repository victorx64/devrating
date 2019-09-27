namespace DevRating.Git
{
    public interface ModificationsFactory
    {
        Modifications Modifications(string sha, string author);
    }
}