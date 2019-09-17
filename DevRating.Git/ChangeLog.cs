namespace DevRating.Git
{
    public interface ChangeLog
    {
        void LogDeletion(string victim, string initiator, string commit);
        void LogAddition(string initiator, string commit);
    }
}