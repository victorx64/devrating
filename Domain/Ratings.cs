namespace DevRating.Domain
{
    public interface Ratings
    {
        Rating Insert(IdObject author,
            double value,
            IdObject work);

        Rating Insert(IdObject author,
            double value,
            IdObject previous,
            IdObject work);

        Rating RatingOf(IdObject author);

        bool ContainsRatingOf(IdObject author);
    }
}