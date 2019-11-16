using DevRating.SqlClient.Entities;

namespace DevRating.SqlClient.Collections
{
    internal interface RatingsCollection
    {
        Rating Insert(IdentifiableObject author,
            double value,
            IdentifiableObject work);

        Rating Insert(IdentifiableObject author,
            double value,
            IdentifiableObject previous,
            IdentifiableObject work);

        Rating LastRatingOf(IdentifiableObject author);

        bool HasRatingOf(IdentifiableObject author);
    }
}