namespace DevRating.Git
{
    public interface Log
    {
        void LogDeletion(int count, string victim, string initiator, string commit);
        void LogAddition(int count, string initiator, string commit);
    }
}