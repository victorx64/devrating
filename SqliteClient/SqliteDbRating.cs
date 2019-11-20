using System.Data;
using DevRating.Database;
using DevRating.Domain;
using Microsoft.Data.Sqlite;

namespace DevRating.SqliteClient
{
    internal sealed class SqliteDbRating : DbRating
    {
        private readonly IDbConnection _connection;
        private readonly long _id;

        public SqliteDbRating(IDbConnection connection, long id)
        {
            _connection = connection;
            _id = id;
        }

        public object Id()
        {
            return _id;
        }

        public Rating PreviousRating()
        {
            using var command = _connection.CreateCommand();

            command.CommandText = "SELECT PreviousRatingId FROM Rating WHERE Id = @Id";

            command.Parameters.Add(new SqliteParameter("@Id", SqliteType.Integer) {Value = _id});

            using var reader = command.ExecuteReader();

            reader.Read();

            return new SqliteDbRating(_connection, (long) reader["PreviousRatingId"]);
        }

        public bool HasPreviousRating()
        {
            using var command = _connection.CreateCommand();

            command.CommandText = "SELECT PreviousRatingId FROM Rating WHERE Id = @Id AND PreviousRatingId IS NOT NULL";

            command.Parameters.Add(new SqliteParameter("@Id", SqliteType.Integer) {Value = _id});

            using var reader = command.ExecuteReader();

            return reader.Read();
        }

        public Work Work()
        {
            using var command = _connection.CreateCommand();

            command.CommandText = "SELECT WorkId FROM Rating WHERE Id = @Id";

            command.Parameters.Add(new SqliteParameter("@Id", SqliteType.Integer) {Value = _id});

            using var reader = command.ExecuteReader();

            reader.Read();

            return new SqliteDbWork(_connection, (long) reader["WorkId"]);
        }

        public Author Author()
        {
            using var command = _connection.CreateCommand();

            command.CommandText = "SELECT AuthorId FROM Rating WHERE Id = @Id";

            command.Parameters.Add(new SqliteParameter("@Id", SqliteType.Integer) {Value = _id});

            using var reader = command.ExecuteReader();

            reader.Read();

            return new SqliteDbAuthor(_connection, (long) reader["AuthorId"]);
        }

        public double Value()
        {
            using var command = _connection.CreateCommand();

            command.CommandText = "SELECT Rating FROM Rating WHERE Id = @Id";

            command.Parameters.Add(new SqliteParameter("@Id", SqliteType.Integer) {Value = _id});

            using var reader = command.ExecuteReader();

            reader.Read();

            return (double) reader["Rating"];
        }
    }
}