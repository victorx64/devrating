using DevRating.Domain;

namespace DevRating.SqlClient.Entities
{
    internal interface IdentifiableReward : Reward, IdentifiableObject
    {
    }
}