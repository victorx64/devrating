using System.Data;
using DevRating.Domain;
using Microsoft.Data.SqlClient;

namespace DevRating.SqlClient.Entities
{
    internal sealed class SqlAuthor : Author, IdentifiableObject
    {
        private readonly IDbConnection _connection;
        private readonly int _id;

        public SqlAuthor(IDbConnection connection, int id)
        {
            _connection = connection;
            _id = id;
        }

        public int Id()
        {
            return _id;
        }

        public string Email()
        {
            using var command = _connection.CreateCommand();

            command.CommandText = "SELECT [Email] FROM [dbo].[Author] WHERE [Id] = @Id";

            command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) {Value = _id});

            using var reader = command.ExecuteReader();

            reader.Read();

            return (string) reader["Email"];
        }

        public Rating LastRating()
        {
            using var command = _connection.CreateCommand();

            command.CommandText =
                "SELECT [LastRatingId] FROM [dbo].[Author] WHERE [Id] = @Id AND [LastRatingId] IS NOT NULL";

            command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) {Value = _id});

            using var reader = command.ExecuteReader();

            reader.Read();

            return new SqlRating(_connection, (int) reader["LastRatingId"]);
        }

        public bool HasRating()
        {
            using var command = _connection.CreateCommand();

            command.CommandText =
                "SELECT [LastRatingId] FROM [dbo].[Author] WHERE [Id] = @Id AND [LastRatingId] IS NOT NULL";

            command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) {Value = _id});

            using var reader = command.ExecuteReader();

            return reader.Read();
        }

        public Reward LastReward()
        {
            using var command = _connection.CreateCommand();

            command.CommandText =
                "SELECT [LastRewardId] FROM [dbo].[Author] WHERE [Id] = @Id AND [LastRewardId] IS NOT NULL";

            command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) {Value = _id});

            using var reader = command.ExecuteReader();

            reader.Read();

            return new SqlReward(_connection, (int) reader["LastRewardId"]);
        }

        public bool HasReward()
        {
            using var command = _connection.CreateCommand();

            command.CommandText =
                "SELECT [LastRewardId] FROM [dbo].[Author] WHERE [Id] = @Id AND [LastRewardId] IS NOT NULL";

            command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) {Value = _id});

            using var reader = command.ExecuteReader();

            return reader.Read();
        }
    }
}