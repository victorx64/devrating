using System.Data;
using Microsoft.Data.SqlClient;

namespace DevRating.SqlClient
{
    public class DbRating : Rating
    {
        private readonly IDbConnection _connection;
        private readonly int _id;

        public DbRating(IDbConnection connection, int id)
        {
            _connection = connection;
            _id = id;
        }

        public int Id()
        {
            return _id;
        }

        public int AuthorId()
        {
            using var command = _connection.CreateCommand();

            command.CommandText = "SELECT [AuthorId] FROM [dbo].[Rating] WHERE [Id] = @Id";

            command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) {Value = _id});

            using var reader = command.ExecuteReader();

            reader.Read();

            return (int) reader["AuthorId"];
        }

        public int LastRatingId()
        {
            using var command = _connection.CreateCommand();

            command.CommandText = "SELECT [LastRatingId] FROM [dbo].[Rating] WHERE [Id] = @Id";

            command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) {Value = _id});

            using var reader = command.ExecuteReader();

            reader.Read();

            return (int) reader["LastRatingId"];
        }

        public bool HasLastRating()
        {
            using var command = _connection.CreateCommand();

            command.CommandText = "SELECT [LastRatingId] FROM [dbo].[Rating] WHERE [Id] = @Id";

            command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) {Value = _id});

            using var reader = command.ExecuteReader();

            return reader.Read();
        }

        public int MatchId()
        {
            using var command = _connection.CreateCommand();

            command.CommandText = "SELECT [MatchId] FROM [dbo].[Rating] WHERE [Id] = @Id";

            command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) {Value = _id});

            using var reader = command.ExecuteReader();

            reader.Read();

            return (int) reader["MatchId"];
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