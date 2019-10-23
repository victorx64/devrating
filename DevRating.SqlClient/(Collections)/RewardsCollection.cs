namespace DevRating.SqlClient
{
    internal interface RewardsCollection
    {
        Reward NewReward(double value, string commit, string repository, int count, int rating);
        Reward NewReward(double value, string commit, string repository, int count);
    }
}