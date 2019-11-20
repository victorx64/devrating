using System.Data;
using DevRating.Database;
using Microsoft.Data.SqlClient;

namespace DevRating.SqlServerClient
{
    internal sealed class SqlServerRatings : Ratings
    {
        private readonly IDbConnection _connection;

        public SqlServerRatings(IDbConnection connection)
        {
            _connection = connection;
        }

        public DbRating Insert(DbObject author, double value, DbObject work)
        {
            using var command = _connection.CreateCommand();

            command.CommandText = @"
                INSERT INTO Rating
                    (Rating
                    ,PreviousRatingId
                    ,WorkId
                    ,AuthorId)
                OUTPUT Inserted.Id
                VALUES
                       (@Rating
                       ,NULL
                       ,@WorkId
                       ,@AuthorId)";

            command.Parameters.Add(new SqlParameter("@Rating", SqlDbType.Real) {Value = value});
            command.Parameters.Add(new SqlParameter("@WorkId", SqlDbType.Int) {Value = work.Id()});
            command.Parameters.Add(new SqlParameter("@AuthorId", SqlDbType.Int) {Value = author.Id()});

            return new SqlServerDbRating(_connection, command.ExecuteScalar());
        }

        public DbRating Insert(DbObject author, double value, DbObject previous, DbObject work)
        {
            using var command = _connection.CreateCommand();

            command.CommandText = @"
                INSERT INTO Rating
                    (Rating
                    ,PreviousRatingId
                    ,WorkId
                    ,AuthorId)
                OUTPUT Inserted.Id
                VALUES
                       (@Rating
                       ,@PreviousRatingId
                       ,@WorkId
                       ,@AuthorId)";

            command.Parameters.Add(new SqlParameter("@Rating", SqlDbType.Real) {Value = value});
            command.Parameters.Add(new SqlParameter("@PreviousRatingId", SqlDbType.Int) {Value = previous.Id()});
            command.Parameters.Add(new SqlParameter("@WorkId", SqlDbType.Int) {Value = work.Id()});
            command.Parameters.Add(new SqlParameter("@AuthorId", SqlDbType.Int) {Value = author.Id()});

            return new SqlServerDbRating(_connection, command.ExecuteScalar());
        }

        public DbRating RatingOf(DbObject author)
        {
            using var command = _connection.CreateCommand();

            command.CommandText =
                "SELECT TOP (1) Id FROM Rating WHERE AuthorId = @AuthorId ORDER BY Id DESC";

            command.Parameters.Add(new SqlParameter("@AuthorId", SqlDbType.Int) {Value = author.Id()});

            using var reader = command.ExecuteReader();

            reader.Read();

            return new SqlServerDbRating(_connection, reader["Id"]);
        }

        public bool HasRatingOf(DbObject author)
        {
            using var command = _connection.CreateCommand();

            command.CommandText =
                "SELECT TOP (1) Id FROM Rating WHERE AuthorId = @AuthorId ORDER BY Id DESC";

            command.Parameters.Add(new SqlParameter("@AuthorId", SqlDbType.Int) {Value = author.Id()});

            using var reader = command.ExecuteReader();

            return reader.Read();
        }
    }
}