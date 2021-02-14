// Copyright (c) 2019-present Viktor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
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

        public Author Insert(string organization, string repository, string email, DateTimeOffset createdAt)
        {
            using var command = _connection.CreateCommand();

            command.CommandText = @"
                INSERT INTO Author
                    (Organization, Repository, Email, CreatedAt)
                VALUES
                    (@Organization, @Repository, @Email, @CreatedAt);
                SELECT last_insert_rowid();";

            command.Parameters.Add(new SqliteParameter("@Email", SqliteType.Text) {Value = email});
            command.Parameters.Add(new SqliteParameter("@Organization", SqliteType.Text) {Value = organization});
            command.Parameters.Add(new SqliteParameter("@Repository", SqliteType.Text) {Value = repository});
            command.Parameters.Add(new SqliteParameter("@CreatedAt", SqliteType.Integer) {Value = createdAt});

            return new SqliteAuthor(_connection, new DefaultId(command.ExecuteScalar()!));
        }
    }
}