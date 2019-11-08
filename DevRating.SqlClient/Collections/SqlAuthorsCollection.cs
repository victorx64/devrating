using System.Collections.Generic;
using System.Data;
using DevRating.SqlClient.Entities;
using Microsoft.Data.SqlClient;

namespace DevRating.SqlClient.Collections
{
    internal sealed class SqlAuthorsCollection : AuthorsCollection
    {
        private readonly IDbConnection _connection;

        public SqlAuthorsCollection(IDbConnection connection)
        {
            _connection = connection;
        }

        public SqlAuthor NewAuthor(string email)
        {
            using var command = _connection.CreateCommand();

            command.CommandText = @"
                INSERT INTO [dbo].[Author]
                       ([Email])
                OUTPUT [Inserted].[Id]
                VALUES
                       (@Email)";

            command.Parameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar, 50) {Value = email});

            return new SqlAuthor(_connection, (int) command.ExecuteScalar());
        }

        public bool Exist(string email)
        {
            using var command = _connection.CreateCommand();

            command.CommandText = "SELECT [Id] FROM [dbo].[Author] WHERE [Email] = @Email";

            command.Parameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar) {Value = email});

            using var reader = command.ExecuteReader();

            return reader.Read();
        }

        public SqlAuthor Author(string email)
        {
            using var command = _connection.CreateCommand();

            command.CommandText = "SELECT [Id] FROM [dbo].[Author] WHERE [Email] = @Email";

            command.Parameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar) {Value = email});

            using var reader = command.ExecuteReader();

            reader.Read();

            return new SqlAuthor(_connection, (int) reader["Id"]);
        }

        public IEnumerable<SqlAuthor> TopAuthors()
        {
            using var command = _connection.CreateCommand();

            command.CommandText = @"
                SELECT TOP (100) 
                    [dbo].[Author].[Id],
                    [dbo].[Author].[Email],
                    [dbo].[Author].[LastRewardId],
                    [dbo].[Reward].[RatingId],
                    [dbo].[Rating].[Rating]
                FROM [dbo].[Author]
                INNER JOIN [dbo].[Reward] ON [dbo].[Reward].[Id] = [dbo].[Author].[LastRewardId]
                INNER JOIN [dbo].[Rating] ON [dbo].[Rating].[Id] = [dbo].[Reward].[RatingId]
                ORDER BY [dbo].[Rating].[Rating] DESC";

            using var reader = command.ExecuteReader();

            var authors = new List<SqlAuthor>();

            while (reader.Read())
            {
                authors.Add(
                    new SqlAuthor(
                        new FakeConnection(
                            new FakeCommand(
                                new FakeDataReader(
                                    new Dictionary<string, object>
                                    {
                                        {"Email", reader["Email"]},
                                        {"LastRewardId", reader["LastRewardId"]},
                                        {"RatingId", reader["RatingId"]},
                                        {"Rating", reader["Rating"]}
                                    }
                                )
                            )
                        ),
                        (int) reader["Id"]));
            }

            return authors;
        }
    }
}