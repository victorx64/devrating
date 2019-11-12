using DevRating.SqlClient.Entities;

namespace DevRating.SqlClient.Collections
{
    internal interface MatchesCollection
    {
        IdentifiableObject Insert(IdentifiableAuthor first,
            IdentifiableAuthor second,
            string commit,
            string repository,
            uint count);
    }
}