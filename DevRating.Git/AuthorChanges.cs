namespace DevRating.Git
{
    public interface AuthorChanges
    {
        void AddChange(string previous, string next, string commit);
        void AddChange(string next, string commit);
    }
}