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

        public Rating Insert(IdentifiableObject author, double value, IdentifiableObject work)
        {
            using var command = _connection.CreateCommand();

            command.CommandText = @"
                INSERT INTO [dbo].[Rating]
                       ([Rating]
                       ,[PreviousRatingId]
                       ,[WorkId]
                       ,[AuthorId])
                OUTPUT [Inserted].[Id]
                VALUES
                       (@Rating
                       ,NULL
                       ,@WorkId
                       ,@AuthorId)";

            command.Parameters.Add(new SqlParameter("@Rating", SqlDbType.Real) {Value = value});
            command.Parameters.Add(new SqlParameter("@WorkId", SqlDbType.Int) {Value = work.Id()});
            command.Parameters.Add(new SqlParameter("@AuthorId", SqlDbType.Int) {Value = author.Id()});

            return new SqlRating(_connection, (int) command.ExecuteScalar());
        }

        public Rating Insert(IdentifiableObject author, double value, IdentifiableObject previous, IdentifiableObject work)
        {
            using var command = _connection.CreateCommand();

            command.CommandText = @"
                INSERT INTO [dbo].[Rating]
                       ([Rating]
                       ,[PreviousRatingId]
                       ,[WorkId]
                       ,[AuthorId])
                OUTPUT [Inserted].[Id]
                VALUES
                       (@Rating
                       ,@PreviousRatingId
                       ,@WorkId
                       ,@AuthorId)";

            command.Parameters.Add(new SqlParameter("@Rating", SqlDbType.Real) {Value = value});
            command.Parameters.Add(new SqlParameter("@PreviousRatingId", SqlDbType.Int) {Value = previous.Id()});
            command.Parameters.Add(new SqlParameter("@WorkId", SqlDbType.Int) {Value = work.Id()});
            command.Parameters.Add(new SqlParameter("@AuthorId", SqlDbType.Int) {Value = author.Id()});

            return new SqlRating(_connection, (int) command.ExecuteScalar());
        }

        public Rating LastRatingOf(IdentifiableObject author)
        {
            using var command = _connection.CreateCommand();

            command.CommandText =
                "SELECT TOP (1) [Id] FROM [dbo].[Rating] WHERE [AuthorId] = @AuthorId ORDER BY [Id] DESC";

            command.Parameters.Add(new SqlParameter("@AuthorId", SqlDbType.Int) {Value = author.Id()});

            using var reader = command.ExecuteReader();

            reader.Read();

            return new SqlRating(_connection, (int) reader["Id"]);
        }

        public bool HasRatingOf(IdentifiableObject author)
        {
            using var command = _connection.CreateCommand();

            command.CommandText =
                "SELECT TOP (1) [Id] FROM [dbo].[Rating] WHERE [AuthorId] = @AuthorId ORDER BY [Id] DESC";

            command.Parameters.Add(new SqlParameter("@AuthorId", SqlDbType.Int) {Value = author.Id()});

            using var reader = command.ExecuteReader();

            return reader.Read();
        }
    }
}