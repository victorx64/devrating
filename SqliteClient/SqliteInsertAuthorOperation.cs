using System.Data;
using DevRating.DefaultObject;
using DevRating.Domain;
using Microsoft.Data.Sqlite;

namespace DevRating.SqliteClient
{
    internal sealed class SqliteInsertAuthorOperation : InsertAuthorOperation
    {
        private readonly IDbConnection _connection;

        public SqliteInsertAuthorOperation(IDbConnection connection)
        {
            _connection = connection;
        }

        public Author Insert(string email)
        {
            using var command = _connection.CreateCommand();

            command.CommandText = @"
                INSERT INTO Author
                    (Email)
                VALUES
                    (@Email);
                SELECT last_insert_rowid();";

            command.Parameters.Add(new SqliteParameter("@Email", SqliteType.Text, 50) {Value = email});

            return new SqliteAuthor(_connection, new DefaultId(command.ExecuteScalar()));
        }
    }
}