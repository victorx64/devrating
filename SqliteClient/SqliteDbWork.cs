using System.Collections.Generic;
using System.Data;
using DevRating.Database;
using DevRating.Domain;
using Microsoft.Data.Sqlite;

namespace DevRating.SqliteClient
{
    internal sealed class SqliteDbWork : DbWork
    {
        private readonly IDbConnection _connection;
        private readonly object _id;

        public SqliteDbWork(IDbConnection connection, object id)
        {
            _connection = connection;
            _id = id;
        }

        public object Id()
        {
            return _id;
        }

        public double Reward()
        {
            using var command = _connection.CreateCommand();

            command.CommandText = "SELECT Reward FROM Work WHERE Id = @Id";

            command.Parameters.Add(new SqliteParameter("@Id", SqliteType.Integer) {Value = _id});

            using var reader = command.ExecuteReader();

            reader.Read();

            return (double) reader["Reward"];
        }

        public Author Author()
        {
            using var command = _connection.CreateCommand();

            command.CommandText = "SELECT AuthorId FROM Work WHERE Id = @Id";

            command.Parameters.Add(new SqliteParameter("@Id", SqliteType.Integer) {Value = _id});

            using var reader = command.ExecuteReader();

            reader.Read();

            return new SqliteDbAuthor(_connection, reader["AuthorId"]);
        }

        public IEnumerable<Rating> Ratings()
        {
            using var command = _connection.CreateCommand();

            command.CommandText = "SELECT Id FROM Rating WHERE WorkId = @Id";

            command.Parameters.Add(new SqliteParameter("@Id", SqliteType.Integer) {Value = _id});

            using var reader = command.ExecuteReader();

            var ratings = new List<Rating>();

            while (reader.Read())
            {
                ratings.Add(new SqliteDbRating(_connection, reader["Id"]));
            }

            return ratings;
        }

        public Rating UsedRating()
        {
            using var command = _connection.CreateCommand();

            command.CommandText = "SELECT UsedRatingId FROM Work WHERE Id = @Id AND UsedRatingId IS NOT NULL";

            command.Parameters.Add(new SqliteParameter("@Id", SqliteType.Integer) {Value = _id});

            using var reader = command.ExecuteReader();

            return new SqliteDbRating(_connection, reader["UsedRatingId"]);
        }

        public bool HasUsedRating()
        {
            using var command = _connection.CreateCommand();

            command.CommandText = "SELECT UsedRatingId FROM Work WHERE Id = @Id AND UsedRatingId IS NOT NULL";

            command.Parameters.Add(new SqliteParameter("@Id", SqliteType.Integer) {Value = _id});

            using var reader = command.ExecuteReader();

            return reader.Read();
        }
    }
}