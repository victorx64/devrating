using System;
using System.Data;
using DevRating.DefaultObject;
using DevRating.Domain;
using Microsoft.Data.Sqlite;

namespace DevRating.SqliteClient
{
    internal sealed class SqliteWork : Work
    {
        private readonly IDbConnection _connection;
        private readonly Id _id;

        public SqliteWork(IDbConnection connection, Id id)
        {
            _connection = connection;
            _id = id;
        }

        public Id Id()
        {
            return _id;
        }

        public string ToJson()
        {
            throw new NotImplementedException();
        }

        public uint Additions()
        {
            using var command = _connection.CreateCommand();

            command.CommandText = "SELECT Additions FROM Work WHERE Id = @Id";

            command.Parameters.Add(new SqliteParameter("@Id", SqliteType.Integer) {Value = _id.Value()});

            using var reader = command.ExecuteReader();

            reader.Read();

            return (uint) (long) reader["Additions"];
        }

        public Author Author()
        {
            using var command = _connection.CreateCommand();

            command.CommandText = "SELECT AuthorId FROM Work WHERE Id = @Id";

            command.Parameters.Add(new SqliteParameter("@Id", SqliteType.Integer) {Value = _id.Value()});

            using var reader = command.ExecuteReader();

            reader.Read();

            return new SqliteAuthor(_connection, new DefaultId(reader["AuthorId"]));
        }

        public Rating UsedRating()
        {
            using var command = _connection.CreateCommand();

            command.CommandText = "SELECT UsedRatingId FROM Work WHERE Id = @Id";

            command.Parameters.Add(new SqliteParameter("@Id", SqliteType.Integer) {Value = _id.Value()});

            using var reader = command.ExecuteReader();

            reader.Read();

            return new SqliteRating(_connection, new DefaultId(reader["UsedRatingId"]));
        }

        public string Repository()
        {
            using var command = _connection.CreateCommand();

            command.CommandText = "SELECT Repository FROM Work WHERE Id = @Id";

            command.Parameters.Add(new SqliteParameter("@Id", SqliteType.Integer) {Value = _id.Value()});

            return (string) command.ExecuteScalar();
        }

        public string Start()
        {
            using var command = _connection.CreateCommand();

            command.CommandText = "SELECT StartCommit FROM Work WHERE Id = @Id";

            command.Parameters.Add(new SqliteParameter("@Id", SqliteType.Integer) {Value = _id.Value()});

            return (string) command.ExecuteScalar();
        }

        public string End()
        {
            using var command = _connection.CreateCommand();

            command.CommandText = "SELECT EndCommit FROM Work WHERE Id = @Id";

            command.Parameters.Add(new SqliteParameter("@Id", SqliteType.Integer) {Value = _id.Value()});

            return (string) command.ExecuteScalar();
        }

        public Envelope Since()
        {
            using var command = _connection.CreateCommand();

            command.CommandText = "SELECT SinceCommit FROM Work WHERE Id = @Id";

            command.Parameters.Add(new SqliteParameter("@Id", SqliteType.Integer) {Value = _id.Value()});

            return new DefaultEnvelope((IConvertible) command.ExecuteScalar());
        }
    }
}