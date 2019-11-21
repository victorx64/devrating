using System.Collections.Generic;
using System.Data;
using DevRating.Database;
using Microsoft.Data.SqlClient;

namespace DevRating.SqlServerClient
{
    internal sealed class SqlServerAuthors : Authors
    {
        private readonly IDbConnection _connection;

        public SqlServerAuthors(IDbConnection connection)
        {
            _connection = connection;
        }

        public DbAuthor Insert(string email)
        {
            using var command = _connection.CreateCommand();

            command.CommandText = @"
                INSERT INTO Author
                    (Email)
                OUTPUT Inserted.Id
                VALUES
                    (@Email)";

            command.Parameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar, 50) {Value = email});

            return new SqlServerDbAuthor(_connection, command.ExecuteScalar());
        }

        public bool Exist(string email)
        {
            using var command = _connection.CreateCommand();

            command.CommandText = "SELECT Id FROM Author WHERE Email = @Email";

            command.Parameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar) {Value = email});

            using var reader = command.ExecuteReader();

            return reader.Read();
        }

        public DbAuthor Author(string email)
        {
            using var command = _connection.CreateCommand();

            command.CommandText = "SELECT Id FROM Author WHERE Email = @Email";

            command.Parameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar) {Value = email});

            using var reader = command.ExecuteReader();

            reader.Read();

            return new SqlServerDbAuthor(_connection, reader["Id"]);
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

            var authors = new List<SqlServerDbAuthor>();

            while (reader.Read())
            {
                authors.Add(
                    new SqlServerDbAuthor(
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