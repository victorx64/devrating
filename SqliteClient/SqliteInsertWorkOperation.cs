// Copyright (c) 2019-present Viktor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

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
            string repository,
            string start,
            string end,
            Envelope since,
            Id author,
            uint additions,
            Id rating,
            Envelope link
        )
        {
            using var command = _connection.CreateCommand();

            command.CommandText = @"
                INSERT INTO Work
                    (Repository
                    ,Link
                    ,StartCommit
                    ,EndCommit
                    ,SinceCommit
                    ,AuthorId
                    ,Additions
                    ,UsedRatingId)
                VALUES
                    (@Repository
                    ,@Link
                    ,@StartCommit
                    ,@EndCommit
                    ,@SinceCommit
                    ,@AuthorId
                    ,@Additions
                    ,@UsedRatingId);
                SELECT last_insert_rowid();";

            command.Parameters.Add(new SqliteParameter("@Repository", SqliteType.Text) {Value = repository});
            command.Parameters.Add(new SqliteParameter("@Link", SqliteType.Text) {Value = link.Value()});
            command.Parameters.Add(new SqliteParameter("@StartCommit", SqliteType.Text, 50) {Value = start});
            command.Parameters.Add(new SqliteParameter("@EndCommit", SqliteType.Text, 50) {Value = end});
            command.Parameters.Add(new SqliteParameter("@SinceCommit", SqliteType.Text, 50) {Value = since.Value()});
            command.Parameters.Add(new SqliteParameter("@AuthorId", SqliteType.Integer) {Value = author.Value()});
            command.Parameters.Add(new SqliteParameter("@Additions", SqliteType.Integer) {Value = additions});
            command.Parameters.Add(new SqliteParameter("@UsedRatingId", SqliteType.Integer) {Value = rating.Value()});

            return new SqliteWork(_connection, new DefaultId(command.ExecuteScalar()));
        }
    }
}