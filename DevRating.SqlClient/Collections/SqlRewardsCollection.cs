using System.Data;
using DevRating.SqlClient.Entities;
using Microsoft.Data.SqlClient;

namespace DevRating.SqlClient.Collections
{
    internal sealed class SqlRewardsCollection : RewardsCollection
    {
        private readonly IDbConnection _connection;

        public SqlRewardsCollection(IDbConnection connection)
        {
            _connection = connection;
        }

        public SqlReward NewReward(double value, string commit, string repository, uint count, int rating, int author)
        {
            using var command = _connection.CreateCommand();

            command.CommandText = @"
                INSERT INTO [dbo].[Reward]
                       ([Reward]
                       ,[Commit]
                       ,[Repository]
                       ,[Count]
                       ,[RatingId]
                       ,[AuthorId])
                OUTPUT [Inserted].[Id]
                VALUES
                       (@Reward
                       ,@Commit
                       ,@Repository
                       ,@Count
                       ,@RatingId
                       ,@AuthorId)";

            command.Parameters.Add(new SqlParameter("@Reward", SqlDbType.Real) {Value = value});
            command.Parameters.Add(new SqlParameter("@Commit", SqlDbType.NVarChar, 50) {Value = commit});
            command.Parameters.Add(new SqlParameter("@Repository", SqlDbType.NVarChar) {Value = repository});
            command.Parameters.Add(new SqlParameter("@Count", SqlDbType.Int) {Value = count});
            command.Parameters.Add(new SqlParameter("@RatingId", SqlDbType.Int) {Value = rating});
            command.Parameters.Add(new SqlParameter("@AuthorId", SqlDbType.Int) {Value = author});

            return new SqlReward(_connection, (int) command.ExecuteScalar());
        }

        public SqlReward NewReward(double value, string commit, string repository, uint count, int author)
        {
            using var command = _connection.CreateCommand();

            command.CommandText = @"
                INSERT INTO [dbo].[Reward]
                       ([Reward]
                       ,[Commit]
                       ,[Repository]
                       ,[Count]
                       ,[RatingId]
                       ,[AuthorId])
                OUTPUT [Inserted].[Id]
                VALUES
                       (@Reward
                       ,@Commit
                       ,@Repository
                       ,@Count
                       ,NULL
                       ,@AuthorId)";

            command.Parameters.Add(new SqlParameter("@Reward", SqlDbType.Real) {Value = value});
            command.Parameters.Add(new SqlParameter("@Commit", SqlDbType.NVarChar, 50) {Value = commit});
            command.Parameters.Add(new SqlParameter("@Repository", SqlDbType.NVarChar) {Value = repository});
            command.Parameters.Add(new SqlParameter("@Count", SqlDbType.Int) {Value = count});
            command.Parameters.Add(new SqlParameter("@AuthorId", SqlDbType.Int) {Value = author});

            var id = (int) command.ExecuteScalar();

            UpdateAuthorLastRewardId(author, id);

            return new SqlReward(_connection, id);
        }

        private void UpdateAuthorLastRewardId(int author, int reward)
        {
            using var command = _connection.CreateCommand();

            command.CommandText = @"
                UPDATE [dbo].[Author]
                SET [LastRewardId] = @RewardId
                WHERE [dbo].[Author].[Id] = @AuthorId";

            command.Parameters.Add(new SqlParameter("@AuthorId", SqlDbType.Int) {Value = author});
            command.Parameters.Add(new SqlParameter("@RewardId", SqlDbType.Int) {Value = reward});

            command.ExecuteNonQuery();
        }
    }
}