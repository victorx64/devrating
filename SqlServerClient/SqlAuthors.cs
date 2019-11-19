using System.Data;
using DevRating.Database;
using Microsoft.Data.SqlClient;

namespace DevRating.SqlServerClient
{
    public sealed class SqlAuthors : Authors
    {
        private readonly IDbConnection _connection;

        public SqlAuthors(IDbConnection connection)
        {
            _connection = connection;
        }

        public IdentifiableAuthor Insert(string email)
        {
            using var command = _connection.CreateCommand();

            command.CommandText = @"
                INSERT INTO Author
                    (Email)
                OUTPUT Inserted.Id
                VALUES
                    (@Email)";

            command.Parameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar, 50) {Value = email});

            return new SqlIdentifiableAuthor(_connection, (int) command.ExecuteScalar());
        }

        public bool Exist(string email)
        {
            using var command = _connection.CreateCommand();

            command.CommandText = "SELECT Id FROM Author WHERE Email = @Email";

            command.Parameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar) {Value = email});

            using var reader = command.ExecuteReader();

            return reader.Read();
        }

        public IdentifiableAuthor Author(string email)
        {
            using var command = _connection.CreateCommand();

            command.CommandText = "SELECT Id FROM Author WHERE Email = @Email";

            command.Parameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar) {Value = email});

            using var reader = command.ExecuteReader();

            reader.Read();

            return new SqlIdentifiableAuthor(_connection, (int) reader["Id"]);
        }
    }
}