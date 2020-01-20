namespace DevRating.Domain
{
    public interface Diff
    {
        Work From(Works works);
        bool PresentIn(Works works);
        void AddTo(EntitiesFactory factory);
    }
}