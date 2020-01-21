using System.Collections.Generic;
using System.Data;
using DevRating.DefaultObject;
using DevRating.Domain;
using Microsoft.Data.Sqlite;

namespace DevRating.SqliteClient
{
    internal sealed class SqliteGetAuthorOperation : GetAuthorOperation
    {
        private readonly IDbConnection _connection;

        public SqliteGetAuthorOperation(IDbConnection connection)
        {
            _connection = connection;
        }

        public Author Author(string email)
        {
            using var command = _connection.CreateCommand();

            command.CommandText = "SELECT Id FROM Author WHERE Email = @Email";

            command.Parameters.Add(new SqliteParameter("@Email", SqliteType.Text) {Value = email});

            using var reader = command.ExecuteReader();

            reader.Read();

            return new SqliteAuthor(_connection, new DefaultId(reader["Id"]));
        }

        public Author Author(Id id)
        {
            return new SqliteAuthor(_connection, id);
        }

        public IEnumerable<Author> Top(string repository)
        {
            using var command = _connection.CreateCommand();

            command.CommandText = @"
                SELECT a.Id
                FROM Author a
                    INNER JOIN Work w1 ON a.Id = w1.AuthorId
                    INNER JOIN Rating r1 ON a.Id = r1.AuthorId
                    LEFT OUTER JOIN Rating r2 ON (a.id = r2.AuthorId AND r1.Id < r2.Id)
                WHERE r2.Id IS NULL
                  AND EXISTS(
                      SELECT AuthorId 
                      FROM Work w
                      WHERE w.AuthorId = a.Id 
                        AND w.Repository = @Repository)
                ORDER BY r1.Rating DESC";

            command.Parameters.Add(new SqliteParameter("@Repository", SqliteType.Text) {Value = repository});

            using var reader = command.ExecuteReader();

            var authors = new List<SqliteAuthor>();

            while (reader.Read())
            {
                authors.Add(new SqliteAuthor(_connection, new DefaultId(reader["Id"])));
            }

            return authors;
        }

        public IEnumerable<Author> Top()
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
                authors.Add(new SqliteAuthor(_connection, new DefaultId(reader["Id"])));
            }

            return authors;
        }
    }
}