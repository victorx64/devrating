using DevRating.Domain;

namespace DevRating.Database
{
    public interface IdentifiableRating : Rating, IdentifiableObject
    {
    }
}