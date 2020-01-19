using System.Data;
using DevRating.Domain;
using Microsoft.Data.Sqlite;

namespace DevRating.SqliteClient
{
    internal sealed class SqliteWork : Work
    {
        private readonly IDbConnection _connection;
        private readonly object _id;

        public SqliteWork(IDbConnection connection, object id)
        {
            _connection = connection;
            _id = id;
        }

        public object Id()
        {
            return _id;
        }

        public string ToJson()
        {
            throw new System.NotImplementedException();
        }

        public uint Additions()
        {
            using var command = _connection.CreateCommand();

            command.CommandText = "SELECT Additions FROM Work WHERE Id = @Id";

            command.Parameters.Add(new SqliteParameter("@Id", SqliteType.Integer) {Value = _id});

            using var reader = command.ExecuteReader();

            reader.Read();

            return (uint) (long) reader["Additions"];
        }

        public Author Author()
        {
            using var command = _connection.CreateCommand();

            command.CommandText = "SELECT AuthorId FROM Work WHERE Id = @Id";

            command.Parameters.Add(new SqliteParameter("@Id", SqliteType.Integer) {Value = _id});

            using var reader = command.ExecuteReader();

            reader.Read();

            return new SqliteAuthor(_connection, reader["AuthorId"]);
        }

        public Rating UsedRating()
        {
            using var command = _connection.CreateCommand();

            command.CommandText = "SELECT UsedRatingId FROM Work WHERE Id = @Id AND UsedRatingId IS NOT NULL";

            command.Parameters.Add(new SqliteParameter("@Id", SqliteType.Integer) {Value = _id});

            using var reader = command.ExecuteReader();

            reader.Read();

            return new SqliteRating(_connection, reader["UsedRatingId"]);
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