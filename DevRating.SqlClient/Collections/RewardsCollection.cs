using DevRating.SqlClient.Entities;

namespace DevRating.SqlClient.Collections
{
    internal interface RewardsCollection
    {
        Reward NewReward(double value, string commit, string repository, uint count, int rating);
        Reward NewReward(double value, string commit, string repository, uint count);
    }
}