using System.Data;
using DevRating.Database;
using Microsoft.Data.Sqlite;

namespace DevRating.SqliteClient
{
    internal sealed class SqliteDbAuthor : DbAuthor
    {
        private readonly IDbConnection _connection;
        private readonly long _id;

        public SqliteDbAuthor(IDbConnection connection, long id)
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

            command.Parameters.Add(new SqliteParameter("@Id", SqliteType.Integer) {Value = _id});

            using var reader = command.ExecuteReader();

            reader.Read();

            return (string) reader["Email"];
        }
    }
}