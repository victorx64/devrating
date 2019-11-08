using DevRating.SqlClient.Entities;

namespace DevRating.SqlClient.Collections
{
    internal interface RewardsCollection
    {
        SqlReward NewReward(double value, string commit, string repository, uint count, int rating, int author);
        SqlReward NewReward(double value, string commit, string repository, uint count, int author);
    }
}