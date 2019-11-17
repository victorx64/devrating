namespace DevRating.SqlClient
{
    internal interface Ratings
    {
        Rating Insert(IdentifiableObject author,
            double value,
            IdentifiableObject work);

        Rating Insert(IdentifiableObject author,
            double value,
            IdentifiableObject previous,
            IdentifiableObject work);

        Rating RatingOf(IdentifiableObject author);

        bool HasRatingOf(IdentifiableObject author);
    }
}