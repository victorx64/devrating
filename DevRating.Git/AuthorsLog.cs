namespace DevRating.Git
{
    public interface AuthorsLog
    {
        void LogAuthorDeletion(string author, string deleter, string commit);
        void LogAuthorAddition(string author, string commit);
    }
}