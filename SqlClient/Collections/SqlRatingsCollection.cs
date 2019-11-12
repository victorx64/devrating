using System.Collections.Generic;
using System.Data;
using DevRating.SqlClient.Entities;
using Microsoft.Data.SqlClient;

namespace DevRating.SqlClient.Collections
{
    internal sealed class SqlRatingsCollection : RatingsCollection
    {
        private readonly IDbConnection _connection;

        public SqlRatingsCollection(IDbConnection connection)
        {
            _connection = connection;
        }

        public IdentifiableRating Insert(IdentifiableAuthor author, double value, IdentifiableObject match)
        {
            using var command = _connection.CreateCommand();

            command.CommandText = @"
                INSERT INTO [dbo].[Rating]
                       ([Rating]
                       ,[LastRatingId]
                       ,[MatchId]
                       ,[AuthorId])
                OUTPUT [Inserted].[Id]
                VALUES
                       (@Rating
                       ,NULL
                       ,@MatchId
                       ,@AuthorId)";

            command.Parameters.Add(new SqlParameter("@Rating", SqlDbType.Real) {Value = value});
            command.Parameters.Add(new SqlParameter("@MatchId", SqlDbType.Int) {Value = match.Id()});
            command.Parameters.Add(new SqlParameter("@AuthorId", SqlDbType.Int) {Value = author.Id()});

            var id = (int) command.ExecuteScalar();

            UpdateAuthorLastRatingId(author, id);

            return new SqlRating(_connection, id);
        }

        public IdentifiableRating Insert(IdentifiableAuthor author, double value, IdentifiableRating last,
            IdentifiableObject match)
        {
            using var command = _connection.CreateCommand();

            command.CommandText = @"
                INSERT INTO [dbo].[Rating]
                       ([Rating]
                       ,[LastRatingId]
                       ,[MatchId]
                       ,[AuthorId])
                OUTPUT [Inserted].[Id]
                VALUES
                       (@Rating
                       ,@LastRatingId
                       ,@MatchId
                       ,@AuthorId)";

            command.Parameters.Add(new SqlParameter("@Rating", SqlDbType.Real) {Value = value});
            command.Parameters.Add(new SqlParameter("@LastRatingId", SqlDbType.Int) {Value = last.Id()});
            command.Parameters.Add(new SqlParameter("@MatchId", SqlDbType.Int) {Value = match.Id()});
            command.Parameters.Add(new SqlParameter("@AuthorId", SqlDbType.Int) {Value = author.Id()});

            var id = (int) command.ExecuteScalar();

            UpdateAuthorLastRatingId(author, id);

            return new SqlRating(_connection, id);
        }

        private void UpdateAuthorLastRatingId(IdentifiableAuthor author, int rating)
        {
            using var command = _connection.CreateCommand();

            command.CommandText = @"
                UPDATE [dbo].[Author]
                SET [LastRatingId] = @RatingId
                WHERE [dbo].[Author].[Id] = @AuthorId";

            command.Parameters.Add(new SqlParameter("@AuthorId", SqlDbType.Int) {Value = author.Id()});
            command.Parameters.Add(new SqlParameter("@RatingId", SqlDbType.Int) {Value = rating});

            command.ExecuteNonQuery();
        }

        public bool HasRatingOf(IdentifiableAuthor author)
        {
            using var command = _connection.CreateCommand();

            command.CommandText =
                "SELECT TOP (1) [Id] FROM [dbo].[Rating] WHERE [AuthorId] = @AuthorId ORDER BY [Id] DESC";

            command.Parameters.Add(new SqlParameter("@AuthorId", SqlDbType.Int) {Value = author.Id()});

            using var reader = command.ExecuteReader();

            return reader.Read();
        }

        public IEnumerable<IdentifiableRating> RatingsOf(string commit, string repository)
        {
            using var command = _connection.CreateCommand();

            command.CommandText = @"
                SELECT [dbo].[Rating].[Id],
                    [dbo].[Rating].[Rating],
                    [dbo].[Rating].[AuthorId],
                    [dbo].[Author].[Email]
                FROM [dbo].[Rating]
                INNER JOIN [dbo].[Author] ON [dbo].[Author].[Id] = [dbo].[Rating].[AuthorId]
                INNER JOIN [dbo].[Match] ON [dbo].[Match].[Id] = [dbo].[Rating].[MatchId]
                WHERE [dbo].[Match].[Commit] = @Commit 
                AND [dbo].[Match].[Repository] = @Repository";

            command.Parameters.Add(new SqlParameter("@Commit", SqlDbType.NVarChar, 50) {Value = commit});
            command.Parameters.Add(new SqlParameter("@Repository", SqlDbType.NVarChar) {Value = repository});

            using var reader = command.ExecuteReader();

            var ratings = new List<IdentifiableRating>();

            while (reader.Read())
            {
                ratings.Add(
                    new SqlRating(
                        new FakeConnection(
                            new FakeCommand(
                                new Dictionary<string, object>
                                {
                                    {"Rating", reader["Rating"]},
                                    {"AuthorId", reader["AuthorId"]},
                                    {"Email", reader["Email"]},
                                }
                            )
                        ),
                        (int) reader["Id"]));
            }

            return ratings;
        }

        public IdentifiableRating LastRatingOf(IdentifiableAuthor author)
        {
            using var command = _connection.CreateCommand();

            command.CommandText =
                "SELECT TOP (1) [Id] FROM [dbo].[Rating] WHERE [AuthorId] = @AuthorId ORDER BY [Id] DESC";

            command.Parameters.Add(new SqlParameter("@AuthorId", SqlDbType.Int) {Value = author.Id()});

            using var reader = command.ExecuteReader();

            reader.Read();

            return new SqlRating(_connection, (int) reader["Id"]);
        }
    }
}