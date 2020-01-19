namespace DevRating.Domain
{
    public interface Diff
    {
        void AddTo(Entities entities, Formula formula);
        Work WorkFrom(Works works);
        bool PresentIn(Works works);
    }
}