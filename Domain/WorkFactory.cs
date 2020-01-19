namespace DevRating.Domain
{
    public interface WorkFactory
    {
        bool PresentIn(Works works);
        Work WorkFrom(Works works);
        Work InsertedWork(Entities entities, Entity author);
    }
}