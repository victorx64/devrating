using System.Collections.Generic;
using System.Data;
using DevRating.SqlClient.Entities;
using Microsoft.Data.SqlClient;

namespace DevRating.SqlClient.Collections
{
    internal sealed class SqlAuthorsCollection : AuthorsCollection
    {
        private readonly IDbTransaction _transaction;

        public SqlAuthorsCollection(IDbTransaction transaction)
        {
            _transaction = transaction;
        }

        public SqlAuthor NewAuthor(string email)
        {
            using var command = _transaction.Connection.CreateCommand();
            command.Transaction = _transaction;

            command.CommandText = @"
                INSERT INTO [dbo].[Author]
                       ([Email])
                OUTPUT [Inserted].[Id]
                VALUES
                       (@Email)";

            command.Parameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar, 50) {Value = email});

            return new SqlAuthor(_transaction, (int) command.ExecuteScalar());
        }

        public bool Exist(string email)
        {
            using var command = _transaction.Connection.CreateCommand();
            command.Transaction = _transaction;

            command.CommandText = "SELECT [Id] FROM [dbo].[Author] WHERE [Email] = @Email";

            command.Parameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar) {Value = email});

            using var reader = command.ExecuteReader();

            return reader.Read();
        }

        public SqlAuthor Author(string email)
        {
            using var command = _transaction.Connection.CreateCommand();
            command.Transaction = _transaction;

            command.CommandText = "SELECT [Id] FROM [dbo].[Author] WHERE [Email] = @Email";

            command.Parameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar) {Value = email});

            using var reader = command.ExecuteReader();

            reader.Read();

            return new SqlAuthor(_transaction, (int) reader["Id"]);
        }

        public IEnumerable<SqlAuthor> TopAuthors()
        {
            using var command = _transaction.Connection.CreateCommand();
            command.Transaction = _transaction;

            command.CommandText = @"
                SELECT TOP (100) [dbo].[Author].[Id]
                FROM [dbo].[Author]
                INNER JOIN [dbo].[Reward] ON [dbo].[Reward].[Id] = [dbo].[Author].[LastRewardId]
                INNER JOIN [dbo].[Rating] ON [dbo].[Rating].[Id] = [dbo].[Reward].[RatingId]
                ORDER BY [dbo].[Rating].[Rating] DESC";

            using var reader = command.ExecuteReader();

            var authors = new List<SqlAuthor>();

            while (reader.Read())
            {
                authors.Add(new SqlAuthor(_transaction, (int) reader["Id"]));
            }

            return authors;
        }
    }
}