using System.Data;
using DevRating.Domain;
using Microsoft.Data.Sqlite;

namespace DevRating.SqliteClient
{
    internal sealed class SqliteAuthor : Author
    {
        private readonly IDbConnection _connection;
        private readonly Id _id;

        public SqliteAuthor(IDbConnection connection, Id id)
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
            throw new System.NotImplementedException();
        }

        public string Email()
        {
            using var command = _connection.CreateCommand();

            command.CommandText = "SELECT Email FROM Author WHERE Id = @Id";

            command.Parameters.Add(new SqliteParameter("@Id", SqliteType.Integer) {Value = _id.Value()});

            using var reader = command.ExecuteReader();

            reader.Read();

            return (string) reader["Email"];
        }
    }
}