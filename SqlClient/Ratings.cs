namespace DevRating.SqlClient
{
    internal interface Ratings
    {
        IdentifiableRating Insert(IdentifiableObject author,
            double value,
            IdentifiableObject work);

        IdentifiableRating Insert(IdentifiableObject author,
            double value,
            IdentifiableObject previous,
            IdentifiableObject work);

        IdentifiableRating RatingOf(IdentifiableObject author);

        bool HasRatingOf(IdentifiableObject author);
    }
}