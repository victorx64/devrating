namespace DevRating.Domain
{
    public interface Diff
    {
        void AddTo(Works works);
        WorkKey Key();
    }
}