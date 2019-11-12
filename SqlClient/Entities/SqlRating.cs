using System.Data;
using DevRating.Domain;
using Microsoft.Data.SqlClient;

namespace DevRating.SqlClient.Entities
{
    internal sealed class SqlRating : IdentifiableRating
    {
        private readonly IDbConnection _connection;
        private readonly int _id;

        public SqlRating(IDbConnection connection, int id)
        {
            _connection = connection;
            _id = id;
        }

        public int Id()
        {
            return _id;
        }

        public Rating LastRating()
        {
            using var command = _connection.CreateCommand();

            command.CommandText = "SELECT [LastRatingId] FROM [dbo].[Rating] WHERE [Id] = @Id";

            command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) {Value = _id});

            using var reader = command.ExecuteReader();

            reader.Read();

            return new SqlRating(_connection, (int) reader["LastRatingId"]);
        }

        public bool HasLastRating()
        {
            using var command = _connection.CreateCommand();
            command.CommandText = "SELECT [LastRatingId] FROM [dbo].[Rating] WHERE [Id] = @Id";

            command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) {Value = _id});

            using var reader = command.ExecuteReader();

            return reader.Read();
        }

        public Author Author()
        {
            using var command = _connection.CreateCommand();

            command.CommandText = "SELECT [AuthorId] FROM [dbo].[Rating] WHERE [Id] = @Id";

            command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) {Value = _id});

            using var reader = command.ExecuteReader();

            reader.Read();

            return new SqlAuthor(_connection, (int) reader["AuthorId"]);
        }

        public double Value()
        {
            using var command = _connection.CreateCommand();

            command.CommandText = "SELECT [Rating] FROM [dbo].[Rating] WHERE [Id] = @Id";

            command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) {Value = _id});

            using var reader = command.ExecuteReader();

            reader.Read();

            return (float) reader["Rating"];
        }
    }
}