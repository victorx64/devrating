// Copyright (c) 2019-present Viktor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Data;
using DevRating.DefaultObject;
using DevRating.Domain;
using Microsoft.Data.Sqlite;

namespace DevRating.SqliteClient
{
    internal sealed class SqliteRating : Rating
    {
        private readonly IDbConnection _connection;
        private readonly Id _id;

        public SqliteRating(IDbConnection connection, Id id)
        {
            _connection = connection;
            _id = id;
        }

        public Id Id()
        {
            return _id;
        }

        public string ToJson()
        {
            throw new NotImplementedException();
        }

        public Rating PreviousRating()
        {
            using var command = _connection.CreateCommand();

            command.CommandText = "SELECT PreviousRatingId FROM Rating WHERE Id = @Id";

            command.Parameters.Add(new SqliteParameter("@Id", SqliteType.Integer) {Value = _id.Value()});

            using var reader = command.ExecuteReader();

            reader.Read();

            return new SqliteRating(_connection, new DefaultId(reader["PreviousRatingId"]));
        }

        public uint? CountedDeletions()
        {
            using var command = _connection.CreateCommand();

            command.CommandText = "SELECT CountedDeletions FROM Rating WHERE Id = @Id";

            command.Parameters.Add(new SqliteParameter("@Id", SqliteType.Integer) {Value = _id.Value()});

            var result = command.ExecuteScalar();

            return result is DBNull ? null : (uint?) (long?) result;
        }

        public uint? IgnoredDeletions()
        {
            using var command = _connection.CreateCommand();

            command.CommandText = "SELECT IgnoredDeletions FROM Rating WHERE Id = @Id";

            command.Parameters.Add(new SqliteParameter("@Id", SqliteType.Integer) {Value = _id.Value()});

            var result = command.ExecuteScalar();

            return result is DBNull ? null : (uint?) (long?) result;
        }

        public Work Work()
        {
            using var command = _connection.CreateCommand();

            command.CommandText = "SELECT WorkId FROM Rating WHERE Id = @Id";

            command.Parameters.Add(new SqliteParameter("@Id", SqliteType.Integer) {Value = _id.Value()});

            using var reader = command.ExecuteReader();

            reader.Read();

            return new SqliteWork(_connection, new DefaultId(reader["WorkId"]));
        }

        public Author Author()
        {
            using var command = _connection.CreateCommand();

            command.CommandText = "SELECT AuthorId FROM Rating WHERE Id = @Id";

            command.Parameters.Add(new SqliteParameter("@Id", SqliteType.Integer) {Value = _id.Value()});

            using var reader = command.ExecuteReader();

            reader.Read();

            return new SqliteAuthor(_connection, new DefaultId(reader["AuthorId"]));
        }

        public double Value()
        {
            using var command = _connection.CreateCommand();

            command.CommandText = "SELECT Rating FROM Rating WHERE Id = @Id";

            command.Parameters.Add(new SqliteParameter("@Id", SqliteType.Integer) {Value = _id.Value()});

            using var reader = command.ExecuteReader();

            reader.Read();

            return (double) reader["Rating"];
        }
    }
}