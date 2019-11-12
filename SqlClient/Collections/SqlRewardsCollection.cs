using System.Collections.Generic;
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

        public IdentifiableReward Insert(double value, string commit, string repository, uint count,
            IdentifiableRating rating, IdentifiableAuthor author)
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
            command.Parameters.Add(new SqlParameter("@RatingId", SqlDbType.Int) {Value = rating.Id()});
            command.Parameters.Add(new SqlParameter("@AuthorId", SqlDbType.Int) {Value = author.Id()});

            var id = (int) command.ExecuteScalar();

            UpdateAuthorLastRewardId(author, id);

            return new SqlReward(_connection, id);
        }

        public IdentifiableReward Insert(double value, string commit, string repository, uint count,
            IdentifiableAuthor author)
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
            command.Parameters.Add(new SqlParameter("@AuthorId", SqlDbType.Int) {Value = author.Id()});

            var id = (int) command.ExecuteScalar();

            UpdateAuthorLastRewardId(author, id);

            return new SqlReward(_connection, id);
        }

        public IEnumerable<IdentifiableReward> RewardsOf(string commit, string repository)
        {
            using var command = _connection.CreateCommand();

            command.CommandText = @"
                SELECT [dbo].[Reward].[Id],
                    [dbo].[Reward].[Reward],
                    [dbo].[Reward].[AuthorId],
                    [dbo].[Author].[Email],
                    [dbo].[Reward].[RatingId],
                    [dbo].[Rating].[Rating]
                FROM [dbo].[Reward]
                INNER JOIN [dbo].[Author] ON [dbo].[Author].[Id] = [dbo].[Reward].[AuthorId]
                LEFT JOIN [dbo].[Rating] ON [dbo].[Rating].[Id] = [dbo].[Reward].[RatingId]
                WHERE [dbo].[Reward].[Commit] = @Commit 
                AND [dbo].[Reward].[Repository] = @Repository";

            command.Parameters.Add(new SqlParameter("@Commit", SqlDbType.NVarChar, 50) {Value = commit});
            command.Parameters.Add(new SqlParameter("@Repository", SqlDbType.NVarChar) {Value = repository});

            using var reader = command.ExecuteReader();

            var rewards = new List<IdentifiableReward>();

            while (reader.Read())
            {
                rewards.Add(
                    new SqlReward(
                        new FakeConnection(
                            new FakeCommand(
                                new Dictionary<string, object>
                                {
                                    {"Reward", reader["Reward"]},
                                    {"AuthorId", reader["AuthorId"]},
                                    {"Email", reader["Email"]},
                                    {"RatingId", reader["RatingId"]},
                                    {"Rating", reader["Rating"]},
                                }
                            )
                        ),
                        (int) reader["Id"]));
            }

            return rewards;
        }

        private void UpdateAuthorLastRewardId(IdentifiableAuthor author, int reward)
        {
            using var command = _connection.CreateCommand();

            command.CommandText = @"
                UPDATE [dbo].[Author]
                SET [LastRewardId] = @RewardId
                WHERE [dbo].[Author].[Id] = @AuthorId";

            command.Parameters.Add(new SqlParameter("@AuthorId", SqlDbType.Int) {Value = author.Id()});
            command.Parameters.Add(new SqlParameter("@RewardId", SqlDbType.Int) {Value = reward});

            command.ExecuteNonQuery();
        }
    }
}