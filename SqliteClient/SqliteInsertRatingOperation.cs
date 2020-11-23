// Copyright (c) 2019-present Viktor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Data;
using DevRating.DefaultObject;
using DevRating.Domain;
using Microsoft.Data.Sqlite;

namespace DevRating.SqliteClient
{
    internal sealed class SqliteInsertRatingOperation : InsertRatingOperation
    {
        private readonly IDbConnection _connection;

        public SqliteInsertRatingOperation(IDbConnection connection)
        {
            _connection = connection;
        }

        public Rating Insert(
            double value,
            Envelope counted,
            Envelope ignored,
            Id previous,
            Id work,
            Id author,
            DateTimeOffset createdAt
        )
        {
            using var command = _connection.CreateCommand();

            command.CommandText = @"
                INSERT INTO Rating
                    (Rating
                    ,CountedDeletions
                    ,IgnoredDeletions
                    ,PreviousRatingId
                    ,WorkId
                    ,AuthorId
                    ,CreatedAt)
                VALUES
                    (@Rating
                    ,@CountedDeletions
                    ,@IgnoredDeletions
                    ,@PreviousRatingId
                    ,@WorkId
                    ,@AuthorId
                    ,@CreatedAt);
                SELECT last_insert_rowid();";

            command.Parameters.Add(new SqliteParameter("@Rating", SqliteType.Real) {Value = value});
            command.Parameters.Add(new SqliteParameter("@PreviousRatingId", SqliteType.Integer)
                {Value = previous.Value()});
            command.Parameters.Add(new SqliteParameter("@WorkId", SqliteType.Integer) {Value = work.Value()});
            command.Parameters.Add(new SqliteParameter("@AuthorId", SqliteType.Integer) {Value = author.Value()});
            command.Parameters.Add(new SqliteParameter("@CountedDeletions", SqliteType.Integer)
                {Value = counted.Value()});
            command.Parameters.Add(new SqliteParameter("@IgnoredDeletions", SqliteType.Integer)
                {Value = ignored.Value()});
            command.Parameters.Add(new SqliteParameter("@CreatedAt", SqliteType.Integer)
                {Value = createdAt});

            return new SqliteRating(_connection, new DefaultId(command.ExecuteScalar()!));
        }
    }
}