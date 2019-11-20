using System.Data;
using DevRating.Database;
using Microsoft.Data.Sqlite;

namespace DevRating.SqliteClient
{
    internal sealed class SqliteRatings : Ratings
    {
        private readonly IDbConnection _connection;

        public SqliteRatings(IDbConnection connection)
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
                VALUES
                    (@Rating
                    ,NULL
                    ,@WorkId
                    ,@AuthorId);
                SELECT last_insert_rowid();";

            command.Parameters.Add(new SqliteParameter("@Rating", SqliteType.Real) {Value = value});
            command.Parameters.Add(new SqliteParameter("@WorkId", SqliteType.Integer) {Value = work.Id()});
            command.Parameters.Add(new SqliteParameter("@AuthorId", SqliteType.Integer) {Value = author.Id()});

            return new SqliteDbRating(_connection, (long) command.ExecuteScalar());
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
                VALUES
                    (@Rating
                    ,@PreviousRatingId
                    ,@WorkId
                    ,@AuthorId);
                SELECT last_insert_rowid();";

            command.Parameters.Add(new SqliteParameter("@Rating", SqliteType.Real) {Value = value});
            command.Parameters.Add(new SqliteParameter("@PreviousRatingId", SqliteType.Integer) {Value = previous.Id()});
            command.Parameters.Add(new SqliteParameter("@WorkId", SqliteType.Integer) {Value = work.Id()});
            command.Parameters.Add(new SqliteParameter("@AuthorId", SqliteType.Integer) {Value = author.Id()});

            return new SqliteDbRating(_connection, (long) command.ExecuteScalar());
        }

        public DbRating RatingOf(DbObject author)
        {
            using var command = _connection.CreateCommand();

            command.CommandText =
                "SELECT Id FROM Rating WHERE AuthorId = @AuthorId ORDER BY Id DESC LIMIT 1";

            command.Parameters.Add(new SqliteParameter("@AuthorId", SqliteType.Integer) {Value = author.Id()});

            using var reader = command.ExecuteReader();

            reader.Read();

            return new SqliteDbRating(_connection, (long) reader["Id"]);
        }

        public bool HasRatingOf(DbObject author)
        {
            using var command = _connection.CreateCommand();

            command.CommandText =
                "SELECT Id FROM Rating WHERE AuthorId = @AuthorId ORDER BY Id DESC LIMIT 1";

            command.Parameters.Add(new SqliteParameter("@AuthorId", SqliteType.Integer) {Value = author.Id()});

            using var reader = command.ExecuteReader();

            return reader.Read();
        }
    }
}