using DevRating.Domain;

namespace DevRating.SqlClient
{
    internal interface IdentifiableRating : Rating, IdentifiableObject
    {
    }
}