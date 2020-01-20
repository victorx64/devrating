namespace DevRating.Domain
{
    public interface DiffFingerprint
    {
        Work From(Works works);
        bool PresentIn(Works works);
        void AddTo(EntitiesFactory factory);
    }
}