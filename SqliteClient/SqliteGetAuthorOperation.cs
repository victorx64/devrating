// Copyright (c) 2019-present Victor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
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

        public Author Author(string organization, string repository, string email)
        {
            using var command = _connection.CreateCommand();

            command.CommandText = @"
                SELECT Id
                FROM Author
                WHERE Organization = @Organization
                AND Repository = @Repository
                AND Email = @Email";

            command.Parameters.Add(new SqliteParameter("@Organization", SqliteType.Text) {Value = organization});
            command.Parameters.Add(new SqliteParameter("@Repository", SqliteType.Text) {Value = repository});
            command.Parameters.Add(new SqliteParameter("@Email", SqliteType.Text) {Value = email});

            using var reader = command.ExecuteReader();

            reader.Read();

            return new SqliteAuthor(_connection, new DefaultId(reader["Id"]));
        }

        public Author Author(Id id)
        {
            return new SqliteAuthor(_connection, id);
        }

        public IEnumerable<Author> Top(string organization, string repository, DateTimeOffset after)
        {
            using var command = _connection.CreateCommand();

            command.CommandText = @"
                SELECT a.Id
                FROM Author a
                         INNER JOIN Rating r1 ON a.Id = r1.AuthorId
                         INNER JOIN Work w ON w.Id = r1.WorkId
                         LEFT OUTER JOIN Rating r2 ON (a.id = r2.AuthorId AND r1.Id < r2.Id)
                WHERE a.Organization = @Organization AND a.Repository = @Repository AND w.CreatedAt > @After
                  AND r2.Id IS NULL
                ORDER BY r1.Rating DESC";

            command.Parameters.Add(new SqliteParameter("@Organization", SqliteType.Text) {Value = organization});
            command.Parameters.Add(new SqliteParameter("@Repository", SqliteType.Text) {Value = repository});
            command.Parameters.Add(new SqliteParameter("@After", SqliteType.Integer) {Value = after});

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