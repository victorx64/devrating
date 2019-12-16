namespace DevRating.Domain
{
    public interface Ratings
    {
        Rating Insert(Entity author,
            double value,
            Entity work);

        Rating Insert(Entity author,
            double value,
            Entity previous,
            Entity work);

        Rating RatingOf(Entity author);
        Rating Rating(string id);

        bool ContainsRatingOf(Entity author);
    }
}