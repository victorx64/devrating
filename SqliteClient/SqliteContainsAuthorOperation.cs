using System.Data;
using DevRating.Domain;
using Microsoft.Data.Sqlite;

namespace DevRating.SqliteClient
{
    internal sealed class SqliteContainsAuthorOperation : ContainsAuthorOperation
    {
        private readonly IDbConnection _connection;

        public SqliteContainsAuthorOperation(IDbConnection connection)
        {
            _connection = connection;
        }

        public bool Contains(string email)
        {
            using var command = _connection.CreateCommand();

            command.CommandText = "SELECT Id FROM Author WHERE Email = @Email";

            command.Parameters.Add(new SqliteParameter("@Email", SqliteType.Text) {Value = email});

            using var reader = command.ExecuteReader();

            return reader.Read();
        }

        public bool Contains(object id)
        {
            using var command = _connection.CreateCommand();

            command.CommandText = "SELECT Id FROM Author WHERE Id = @Id";

            command.Parameters.Add(new SqliteParameter("@Id", SqliteType.Integer) {Value = id});

            using var reader = command.ExecuteReader();

            return reader.Read();
        }
    }
}