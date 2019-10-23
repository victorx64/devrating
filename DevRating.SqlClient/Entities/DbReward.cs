using System.Data;

namespace DevRating.SqlClient.Entities
{
    internal class DbReward : Reward
    {
        private readonly int _id;

        public DbReward(IDbTransaction transaction, int id)
        {
            _id = id;
        }

        public int Id()
        {
            return _id;
        }
    }
}