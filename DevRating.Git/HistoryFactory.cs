namespace DevRating.Git
{
    public interface HistoryFactory
    {
        History History(string commit, string author);
    }
}