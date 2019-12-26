namespace DevRating.Domain
{
    public interface Diff
    {
        void AddTo(Diffs diffs);
        Work WorkFrom(Works works);
        bool ExistIn(Works works);
    }
}