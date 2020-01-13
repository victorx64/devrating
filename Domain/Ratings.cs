namespace DevRating.Domain
{
    public interface Ratings
    {
        InsertRatingOperation InsertOperation();

        Rating RatingOf(Entity author);
        Rating Rating(object id);

        bool ContainsRatingOf(Entity author);
        bool Contains(object id);
    }
}