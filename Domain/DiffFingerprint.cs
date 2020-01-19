namespace DevRating.Domain
{
    public interface DiffFingerprint
    {
        void AddTo(Entities entities, Formula formula);
        Work WorkFrom(Works works);
        bool PresentIn(Works works);
    }
}