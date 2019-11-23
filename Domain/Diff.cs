namespace DevRating.Domain
{
    public interface Diff
    {
        void AddTo(Storage storage);
        WorkKey Key();
    }
}