using System.Data;
using DevRating.Database;
using DevRating.Domain;
using Microsoft.Data.SqlClient;

namespace DevRating.SqlServerClient
{
    internal sealed class SqlServerDbAuthor : DbAuthor
    {
        private readonly IDbConnection _connection;
        private readonly object _id;

        public SqlServerDbAuthor(IDbConnection connection, object id)
        {
            _connection = connection;
            _id = id;
        }

        public object Id()
        {
            return _id;
        }

        public string Email()
        {
            using var command = _connection.CreateCommand();

            command.CommandText = "SELECT Email FROM Author WHERE Id = @Id";

            command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) {Value = _id});

            using var reader = command.ExecuteReader();

            reader.Read();

            return (string) reader["Email"];
        }

        public Rating Rating()
        {
            using var command = _connection.CreateCommand();

            command.CommandText = "SELECT TOP (1) Id FROM Rating WHERE Rating.AuthorId = @AuthorId ORDER BY Id DESC";

            command.Parameters.Add(new SqlParameter("@AuthorId", SqlDbType.Int) {Value = _id});

            using var reader = command.ExecuteReader();

            reader.Read();

            return new SqlServerDbRating(_connection, reader["Id"]);
        }

        public bool HasRating()
        {
            using var command = _connection.CreateCommand();

            command.CommandText = "SELECT TOP (1) Id FROM Rating WHERE Rating.AuthorId = @AuthorId ORDER BY Id DESC";

            command.Parameters.Add(new SqlParameter("@AuthorId", SqlDbType.Int) {Value = _id});

            using var reader = command.ExecuteReader();

            return reader.Read();
        }
    }
}