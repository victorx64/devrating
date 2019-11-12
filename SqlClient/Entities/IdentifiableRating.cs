using DevRating.Domain;

namespace DevRating.SqlClient.Entities
{
    internal interface IdentifiableRating : Rating, IdentifiableObject
    {
    }
}