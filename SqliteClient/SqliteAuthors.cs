using System.Data;
using DevRating.Database;
using Microsoft.Data.Sqlite;

namespace DevRating.SqliteClient
{
    internal sealed class SqliteAuthors : Authors
    {
        private readonly IDbConnection _connection;

        public SqliteAuthors(IDbConnection connection)
        {
            _connection = connection;
        }

        public DbAuthor Insert(string email)
        {
            using var command = _connection.CreateCommand();

            command.CommandText = @"
                INSERT INTO Author
                    (Email)
                VALUES
                    (@Email);
                SELECT last_insert_rowid();";

            command.Parameters.Add(new SqliteParameter("@Email", SqliteType.Text, 50) {Value = email});

            return new SqliteDbAuthor(_connection, (long) command.ExecuteScalar());
        }

        public bool Exist(string email)
        {
            using var command = _connection.CreateCommand();

            command.CommandText = "SELECT Id FROM Author WHERE Email = @Email";

            command.Parameters.Add(new SqliteParameter("@Email", SqliteType.Text) {Value = email});

            using var reader = command.ExecuteReader();

            return reader.Read();
        }

        public DbAuthor Author(string email)
        {
            using var command = _connection.CreateCommand();

            command.CommandText = "SELECT Id FROM Author WHERE Email = @Email";

            command.Parameters.Add(new SqliteParameter("@Email", SqliteType.Text) {Value = email});

            using var reader = command.ExecuteReader();

            reader.Read();

            return new SqliteDbAuthor(_connection, (long) reader["Id"]);
        }
    }
}