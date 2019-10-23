using System.Data;
using Microsoft.Data.SqlClient;

namespace DevRating.SqlClient
{
    internal class DbRewardsCollection : RewardsCollection
    {
        private readonly IDbTransaction _transaction;

        public DbRewardsCollection(IDbTransaction transaction)
        {
            _transaction = transaction;
        }
        
        public Reward NewReward(double value, string commit, string repository, int count, int rating)
        {
            using var command = _transaction.Connection.CreateCommand();
            command.Transaction = _transaction;

            command.CommandText = @"
                INSERT INTO [dbo].[Reward]
                       ([Reward]
                       ,[Commit]
                       ,[Repository]
                       ,[Count]
                       ,[RatingId])
                OUTPUT [Inserted].[Id]
                VALUES
                       (@Reward
                       ,@Commit
                       ,@Repository
                       ,@Count
                       ,@RatingId)";

            command.Parameters.Add(new SqlParameter("@Reward", SqlDbType.Real) {Value = value});
            command.Parameters.Add(new SqlParameter("@Commit", SqlDbType.NVarChar, 50) {Value = commit});
            command.Parameters.Add(new SqlParameter("@Repository", SqlDbType.NVarChar) {Value = repository});
            command.Parameters.Add(new SqlParameter("@Count", SqlDbType.Int) {Value = count});
            command.Parameters.Add(new SqlParameter("@RatingId", SqlDbType.Int) {Value = rating});

            return new DbReward(_transaction, (int) command.ExecuteScalar());
        }

        public Reward NewReward(double value, string commit, string repository, int count)
        {
            using var command = _transaction.Connection.CreateCommand();
            command.Transaction = _transaction;

            command.CommandText = @"
                INSERT INTO [dbo].[Reward]
                       ([Reward]
                       ,[Commit]
                       ,[Repository]
                       ,[Count]
                       ,[RatingId])
                OUTPUT [Inserted].[Id]
                VALUES
                       (@Reward
                       ,@Commit
                       ,@Repository
                       ,@Count
                       ,NULL)";

            command.Parameters.Add(new SqlParameter("@Reward", SqlDbType.Real) {Value = value});
            command.Parameters.Add(new SqlParameter("@Commit", SqlDbType.NVarChar, 50) {Value = commit});
            command.Parameters.Add(new SqlParameter("@Repository", SqlDbType.NVarChar) {Value = repository});
            command.Parameters.Add(new SqlParameter("@Count", SqlDbType.Int) {Value = count});

            return new DbReward(_transaction, (int) command.ExecuteScalar());
        }
    }
}