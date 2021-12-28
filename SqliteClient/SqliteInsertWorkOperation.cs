// Copyright (c) 2019-present Victor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Data;
using DevRating.DefaultObject;
using DevRating.Domain;
using Microsoft.Data.Sqlite;

namespace DevRating.SqliteClient
{
    internal sealed class SqliteInsertWorkOperation : InsertWorkOperation
    {
        private readonly IDbConnection _connection;

        public SqliteInsertWorkOperation(IDbConnection connection)
        {
            _connection = connection;
        }

        public Work Insert(
            string start,
            string end,
            string? since,
            Id author,
            uint additions,
            Id rating,
            string? link,
            DateTimeOffset createdAt
        )
        {
            using var command = _connection.CreateCommand();

            command.CommandText = @"
                INSERT INTO Work
                    (Link
                    ,StartCommit
                    ,EndCommit
                    ,SinceCommit
                    ,AuthorId
                    ,Additions
                    ,UsedRatingId
                    ,CreatedAt)
                VALUES
                    (@Link
                    ,@StartCommit
                    ,@EndCommit
                    ,@SinceCommit
                    ,@AuthorId
                    ,@Additions
                    ,@UsedRatingId
                    ,@CreatedAt);
                SELECT last_insert_rowid();";

            command.Parameters.Add(new SqliteParameter("@Link", SqliteType.Text) {Value = link ?? (object) DBNull.Value});
            command.Parameters.Add(new SqliteParameter("@StartCommit", SqliteType.Text, 50) {Value = start});
            command.Parameters.Add(new SqliteParameter("@EndCommit", SqliteType.Text, 50) {Value = end});
            command.Parameters.Add(new SqliteParameter("@SinceCommit", SqliteType.Text, 50) {Value = since ?? (object) DBNull.Value});
            command.Parameters.Add(new SqliteParameter("@AuthorId", SqliteType.Integer) {Value = author.Value()});
            command.Parameters.Add(new SqliteParameter("@Additions", SqliteType.Integer) {Value = additions});
            command.Parameters.Add(new SqliteParameter("@UsedRatingId", SqliteType.Integer) {Value = rating.Value()});
            command.Parameters.Add(new SqliteParameter("@CreatedAt", SqliteType.Integer) {Value = createdAt});

            return new SqliteWork(_connection, new DefaultId(command.ExecuteScalar()!));
        }
    }
}