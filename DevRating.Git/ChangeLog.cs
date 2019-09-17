namespace DevRating.Git
{
    public interface ChangeLog
    {
        void LogDeletion(string victim, string initiator, string commit);
        void LogAddition(int count, string initiator, string commit);
    }
}