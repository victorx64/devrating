using DevRating.SqlClient.Entities;

namespace DevRating.SqlClient.Collections
{
    internal interface RewardsCollection
    {
        SqlReward NewReward(double value, string commit, string repository, uint count, int rating);
        SqlReward NewReward(double value, string commit, string repository, uint count);
    }
}