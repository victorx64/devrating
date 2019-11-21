using System.Collections.Generic;
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

            return new SqliteDbAuthor(_connection, command.ExecuteScalar());
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

            return new SqliteDbAuthor(_connection, reader["Id"]);
        }

        public IEnumerable<DbAuthor> TopAuthors()
        {
            using var command = _connection.CreateCommand();

            command.CommandText = @"
                SELECT a.Id,
                       a.Email,
                       r1.Rating,
                       r1.Id RatingId
                FROM Author a
                         INNER JOIN Rating r1 ON a.Id = r1.AuthorId
                         LEFT OUTER JOIN Rating r2 ON (a.id = r2.AuthorId AND r1.Id < r2.Id)
                WHERE r2.Id IS NULL
                ORDER BY r1.Rating DESC";

            using var reader = command.ExecuteReader();

            var authors = new List<SqliteDbAuthor>();

            while (reader.Read())
            {
                authors.Add(
                    new SqliteDbAuthor(
                        new FakeConnection(
                            new FakeCommand(
                                new Dictionary<string, object>
                                {
                                    {"Email", reader["Email"]},
                                    {"Id", reader["RatingId"]},
                                    {"Rating", reader["Rating"]}
                                }
                            )
                        ),
                        reader["Id"]));
            }

            return authors;
        }
    }
}