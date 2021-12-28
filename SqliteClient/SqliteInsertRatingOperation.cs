// Copyright (c) 2019-present Victor Semenov
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
            uint? counted,
            uint? ignored,
            Id previous,
            Id work,
            Id author
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
                    ,AuthorId)
                VALUES
                    (@Rating
                    ,@CountedDeletions
                    ,@IgnoredDeletions
                    ,@PreviousRatingId
                    ,@WorkId
                    ,@AuthorId);
                SELECT last_insert_rowid();";

            command.Parameters.Add(new SqliteParameter("@Rating", SqliteType.Real) {Value = value});
            command.Parameters.Add(new SqliteParameter("@PreviousRatingId", SqliteType.Integer)
                {Value = previous.Value()});
            command.Parameters.Add(new SqliteParameter("@WorkId", SqliteType.Integer) {Value = work.Value()});
            command.Parameters.Add(new SqliteParameter("@AuthorId", SqliteType.Integer) {Value = author.Value()});
            command.Parameters.Add(new SqliteParameter("@CountedDeletions", SqliteType.Integer)
                {Value = counted ?? (object) DBNull.Value});
            command.Parameters.Add(new SqliteParameter("@IgnoredDeletions", SqliteType.Integer)
                {Value = ignored ?? (object) DBNull.Value});

            return new SqliteRating(_connection, new DefaultId(command.ExecuteScalar()!));
        }
    }
}