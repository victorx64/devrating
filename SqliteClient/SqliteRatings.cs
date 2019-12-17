using System.Data;
using DevRating.Domain;
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

        public Rating Insert(Entity author, double value, Entity work)
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

            return new SqliteRating(_connection, command.ExecuteScalar());
        }

        public Rating Insert(Entity author, double value, Entity previous, Entity work)
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
            command.Parameters.Add(new SqliteParameter("@PreviousRatingId", SqliteType.Integer)
                {Value = previous.Id()});
            command.Parameters.Add(new SqliteParameter("@WorkId", SqliteType.Integer) {Value = work.Id()});
            command.Parameters.Add(new SqliteParameter("@AuthorId", SqliteType.Integer) {Value = author.Id()});

            return new SqliteRating(_connection, command.ExecuteScalar());
        }

        public Rating RatingOf(Entity author)
        {
            using var command = _connection.CreateCommand();

            command.CommandText =
                "SELECT Id FROM Rating WHERE AuthorId = @AuthorId ORDER BY Id DESC LIMIT 1";

            command.Parameters.Add(new SqliteParameter("@AuthorId", SqliteType.Integer) {Value = author.Id()});

            using var reader = command.ExecuteReader();

            reader.Read();

            return new SqliteRating(_connection, reader["Id"]);
        }

        public Rating Rating(object id)
        {
            return new SqliteRating(_connection, id);
        }

        public bool Contains(object id)
        {
            using var command = _connection.CreateCommand();

            command.CommandText = "SELECT Id FROM Rating WHERE Id = @Id";

            command.Parameters.Add(new SqliteParameter("@Id", SqliteType.Integer) {Value = id});

            using var reader = command.ExecuteReader();

            return reader.Read();
        }

        public bool ContainsRatingOf(Entity author)
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