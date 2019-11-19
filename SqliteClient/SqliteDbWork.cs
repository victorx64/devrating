using System.Collections.Generic;
using System.Data;
using DevRating.Database;
using DevRating.Domain;
using Microsoft.Data.Sqlite;

namespace DevRating.SqliteClient
{
    public sealed class SqliteDbWork : DbWork
    {
        private readonly IDbConnection _connection;
        private readonly int _id;

        public SqliteDbWork(IDbConnection connection, int id)
        {
            _connection = connection;
            _id = id;
        }

        public int Id()
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

            return (float) reader["Reward"];
        }

        public Author Author()
        {
            using var command = _connection.CreateCommand();

            command.CommandText = "SELECT AuthorId FROM Work WHERE Id = @Id";

            command.Parameters.Add(new SqliteParameter("@Id", SqliteType.Integer) {Value = _id});

            using var reader = command.ExecuteReader();

            reader.Read();

            return new SqliteDbAuthor(_connection, (int) reader["AuthorId"]);
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
                ratings.Add(new SqliteDbRating(_connection, (int) reader["Id"]));
            }

            return ratings;
        }
    }
}