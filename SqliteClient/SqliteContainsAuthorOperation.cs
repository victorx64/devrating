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

        public bool Contains(string organization, string email)
        {
            using var command = _connection.CreateCommand();

            command.CommandText = "SELECT Id FROM Author WHERE Email = @Email AND Organization = @Organization";

            command.Parameters.Add(new SqliteParameter("@Email", SqliteType.Text) {Value = email});
            command.Parameters.Add(new SqliteParameter("@Organization", SqliteType.Text) {Value = organization});

            using var reader = command.ExecuteReader();

            return reader.Read();
        }

        public bool Contains(Id id)
        {
            using var command = _connection.CreateCommand();

            command.CommandText = "SELECT Id FROM Author WHERE Id = @Id";

            command.Parameters.Add(new SqliteParameter("@Id", SqliteType.Integer) {Value = id.Value()});

            using var reader = command.ExecuteReader();

            return reader.Read();
        }
    }
}