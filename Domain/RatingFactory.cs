namespace DevRating.Domain
{
    public interface RatingFactory
    {
        void InsertNewRatings(Entities entities, Formula formula, Entity author, Entity work);
    }
}