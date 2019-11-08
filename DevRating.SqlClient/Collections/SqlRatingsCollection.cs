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

        public SqlRating NewRating(int author, double value, int match)
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
            command.Parameters.Add(new SqlParameter("@MatchId", SqlDbType.Int) {Value = match});
            command.Parameters.Add(new SqlParameter("@AuthorId", SqlDbType.Int) {Value = author});

            var id = (int) command.ExecuteScalar();

            UpdateAuthorLastRatingId(author, id);

            return new SqlRating(_connection, id);
        }

        public SqlRating NewRating(int author, double value, int last, int match)
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
            command.Parameters.Add(new SqlParameter("@LastRatingId", SqlDbType.Int) {Value = last});
            command.Parameters.Add(new SqlParameter("@MatchId", SqlDbType.Int) {Value = match});
            command.Parameters.Add(new SqlParameter("@AuthorId", SqlDbType.Int) {Value = author});

            var id = (int) command.ExecuteScalar();

            UpdateAuthorLastRatingId(author, id);

            return new SqlRating(_connection, id);
        }

        private void UpdateAuthorLastRatingId(int author, int rating)
        {
            using var command = _connection.CreateCommand();

            command.CommandText = @"
                UPDATE [dbo].[Author]
                SET [LastRatingId] = @RatingId
                WHERE [dbo].[Author].[Id] = @AuthorId";

            command.Parameters.Add(new SqlParameter("@AuthorId", SqlDbType.Int) {Value = author});
            command.Parameters.Add(new SqlParameter("@RatingId", SqlDbType.Int) {Value = rating});

            command.ExecuteNonQuery();
        }

        public bool HasRating(int author)
        {
            using var command = _connection.CreateCommand();

            command.CommandText =
                "SELECT TOP (1) [Id] FROM [dbo].[Rating] WHERE [AuthorId] = @AuthorId ORDER BY [Id] DESC";

            command.Parameters.Add(new SqlParameter("@AuthorId", SqlDbType.Int) {Value = author});

            using var reader = command.ExecuteReader();

            return reader.Read();
        }

        public SqlRating LastRatingOf(int author)
        {
            using var command = _connection.CreateCommand();

            command.CommandText =
                "SELECT TOP (1) [Id] FROM [dbo].[Rating] WHERE [AuthorId] = @AuthorId ORDER BY [Id] DESC";

            command.Parameters.Add(new SqlParameter("@AuthorId", SqlDbType.Int) {Value = author});

            using var reader = command.ExecuteReader();

            reader.Read();

            return new SqlRating(_connection, (int) reader["Id"]);
        }
    }
}