using System.Data;
using Microsoft.Data.SqlClient;

namespace DevRating.SqlClient
{
    internal sealed class SqlAuthors : Authors
    {
        private readonly IDbConnection _connection;

        public SqlAuthors(IDbConnection connection)
        {
            _connection = connection;
        }

        public Author Insert(string email)
        {
            using var command = _connection.CreateCommand();

            command.CommandText = @"
                INSERT INTO [dbo].[Author]
                       ([Email])
                OUTPUT [Inserted].[Id]
                VALUES
                       (@Email)";

            command.Parameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar, 50) {Value = email});

            return new SqlAuthor(_connection, (int) command.ExecuteScalar());
        }

        public bool Exist(string email)
        {
            using var command = _connection.CreateCommand();

            command.CommandText = "SELECT [Id] FROM [dbo].[Author] WHERE [Email] = @Email";

            command.Parameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar) {Value = email});

            using var reader = command.ExecuteReader();

            return reader.Read();
        }

        public Author Author(string email)
        {
            using var command = _connection.CreateCommand();

            command.CommandText = "SELECT [Id] FROM [dbo].[Author] WHERE [Email] = @Email";

            command.Parameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar) {Value = email});

            using var reader = command.ExecuteReader();

            reader.Read();

            return new SqlAuthor(_connection, (int) reader["Id"]);
        }
    }
}