// Copyright (c) 2019-present Viktor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Data;
using DevRating.DefaultObject;
using DevRating.Domain;
using Microsoft.Data.Sqlite;

namespace DevRating.SqliteClient
{
    internal sealed class SqliteGetRatingOperation : GetRatingOperation
    {
        private readonly IDbConnection _connection;

        public SqliteGetRatingOperation(IDbConnection connection)
        {
            _connection = connection;
        }

        public Rating RatingOf(Id author)
        {
            using var command = _connection.CreateCommand();

            command.CommandText =
                "SELECT Id FROM Rating WHERE AuthorId = @AuthorId ORDER BY Id DESC LIMIT 1";

            command.Parameters.Add(new SqliteParameter("@AuthorId", SqliteType.Integer) {Value = author.Value()});

            using var reader = command.ExecuteReader();

            return new SqliteRating(_connection, reader.Read() ? new DefaultId(reader["Id"]) : new DefaultId());
        }

        public Rating Rating(Id id)
        {
            return new SqliteRating(_connection, id);
        }

        public IEnumerable<Rating> RatingsOf(Id work)
        {
            using var command = _connection.CreateCommand();

            command.CommandText = "SELECT Id FROM Rating WHERE WorkId = @WorkId";

            command.Parameters.Add(new SqliteParameter("@WorkId", SqliteType.Integer) {Value = work.Value()});

            using var reader = command.ExecuteReader();

            var ratings = new List<Rating>();

            while (reader.Read())
            {
                ratings.Add(new SqliteRating(_connection, new DefaultId(reader["Id"])));
            }

            return ratings;
        }

        public IEnumerable<Rating> Last(Id author, DateTimeOffset after)
        {
            using var command = _connection.CreateCommand();

            command.CommandText = "SELECT Id FROM Rating WHERE AuthorId = @AuthorId AND CreatedAt >= @After";

            command.Parameters.Add(new SqliteParameter("@AuthorId", SqliteType.Integer) {Value = author.Value()});
            command.Parameters.Add(new SqliteParameter("@After", SqliteType.Integer) {Value = after});

            using var reader = command.ExecuteReader();

            var ratings = new List<Rating>();

            while (reader.Read())
            {
                ratings.Add(new SqliteRating(_connection, new DefaultId(reader["Id"])));
            }

            return ratings;
        }
    }
}