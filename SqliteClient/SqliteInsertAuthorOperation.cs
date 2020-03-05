// Copyright (c) 2019-present Viktor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

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

        public Author Insert(string organization, string email)
        {
            using var command = _connection.CreateCommand();

            command.CommandText = @"
                INSERT INTO Author
                    (Organization, Email)
                VALUES
                    (@Organization, @Email);
                SELECT last_insert_rowid();";

            command.Parameters.Add(new SqliteParameter("@Email", SqliteType.Text, 50) {Value = email});
            command.Parameters.Add(new SqliteParameter("@Organization", SqliteType.Text, 50) {Value = organization});

            return new SqliteAuthor(_connection, new DefaultId(command.ExecuteScalar()));
        }
    }
}