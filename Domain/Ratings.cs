namespace DevRating.Domain
{
    public interface Ratings
    {
        RatingInsertOperation InsertOperation();

        Rating RatingOf(Entity author);
        Rating Rating(object id);

        bool ContainsRatingOf(Entity author);
        bool Contains(object id);
    }
}