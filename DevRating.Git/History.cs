namespace DevRating.Git
{
    public interface History
    {
        void LogDeletion(string victim);
        void LogAdditions(int count);
    }
}