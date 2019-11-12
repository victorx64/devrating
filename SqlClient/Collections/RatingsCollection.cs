using System.Collections.Generic;
using DevRating.SqlClient.Entities;

namespace DevRating.SqlClient.Collections
{
    internal interface RatingsCollection
    {
        IdentifiableRating Insert(IdentifiableAuthor author,
            double value,
            IdentifiableObject match);

        IdentifiableRating Insert(IdentifiableAuthor author,
            double value,
            IdentifiableRating last,
            IdentifiableObject match);

        IdentifiableRating LastRatingOf(IdentifiableAuthor author);
        bool HasRatingOf(IdentifiableAuthor author);
        IEnumerable<IdentifiableRating> RatingsOf(string commit, string repository);
    }
}