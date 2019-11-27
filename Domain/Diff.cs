namespace DevRating.Domain
{
    public interface Diff
    {
        void AddTo(Storage storage);
        string Key();
        string StartCommit();
        string EndCommit();
    }
}