namespace DevRating.Domain
{
    public interface Diff
    {
        void AddTo(WorksRepository works);
        WorkKey Key();
    }
}