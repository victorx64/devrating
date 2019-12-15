using System.Collections.Generic;
using System.Data;
using DevRating.Domain;
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

            return new SqliteAuthor(_connection, command.ExecuteScalar());
        }

        public bool Contains(string email)
        {
            using var command = _connection.CreateCommand();

            command.CommandText = "SELECT Id FROM Author WHERE Email = @Email";

            command.Parameters.Add(new SqliteParameter("@Email", SqliteType.Text) {Value = email});

            using var reader = command.ExecuteReader();

            return reader.Read();
        }

        public Author Author(string email)
        {
            using var command = _connection.CreateCommand();

            command.CommandText = "SELECT Id FROM Author WHERE Email = @Email";

            command.Parameters.Add(new SqliteParameter("@Email", SqliteType.Text) {Value = email});

            using var reader = command.ExecuteReader();

            reader.Read();

            return new SqliteAuthor(_connection, reader["Id"]);
        }

        public IEnumerable<Author> TopAuthors()
        {
            using var command = _connection.CreateCommand();

            command.CommandText = @"
                SELECT a.Id
                FROM Author a
                         INNER JOIN Rating r1 ON a.Id = r1.AuthorId
                         LEFT OUTER JOIN Rating r2 ON (a.id = r2.AuthorId AND r1.Id < r2.Id)
                WHERE r2.Id IS NULL
                ORDER BY r1.Rating DESC";

            using var reader = command.ExecuteReader();

            var authors = new List<SqliteAuthor>();

            while (reader.Read())
            {
                authors.Add(new SqliteAuthor(_connection, reader["Id"]));
            }

            return authors;
        }
    }
}