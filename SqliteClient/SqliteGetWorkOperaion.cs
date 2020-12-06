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
    internal sealed class SqliteGetWorkOperation : GetWorkOperation
    {
        private readonly IDbConnection _connection;

        public SqliteGetWorkOperation(IDbConnection connection)
        {
            _connection = connection;
        }

        public Work Work(string repository, string start, string end)
        {
            using var command = _connection.CreateCommand();

            command.CommandText = @"
                SELECT Id 
                FROM Work 
                WHERE Repository = @Repository 
                AND StartCommit = @StartCommit
                AND EndCommit = @EndCommit";

            command.Parameters.Add(new SqliteParameter("@Repository", SqliteType.Text) {Value = repository});
            command.Parameters.Add(new SqliteParameter("@StartCommit", SqliteType.Text, 50) {Value = start});
            command.Parameters.Add(new SqliteParameter("@EndCommit", SqliteType.Text, 50) {Value = end});

            using var reader = command.ExecuteReader();

            reader.Read();

            return new SqliteWork(_connection, new DefaultId(reader["Id"]));
        }

        public Work Work(Id id)
        {
            return new SqliteWork(_connection, id);
        }

        public IEnumerable<Work> Last(string repository, DateTimeOffset after)
        {
            using var command = _connection.CreateCommand();

            command.CommandText = @"
                SELECT w.Id
                FROM Work w
                WHERE w.Repository = @Repository AND w.CreatedAt >= @After
                ORDER BY w.Id DESC";

            command.Parameters.Add(new SqliteParameter("@Repository", SqliteType.Text) {Value = repository});
            command.Parameters.Add(new SqliteParameter("@After", SqliteType.Integer) {Value = after});

            using var reader = command.ExecuteReader();

            var works = new List<SqliteWork>();

            while (reader.Read())
            {
                works.Add(new SqliteWork(_connection, new DefaultId(reader["Id"])));
            }

            return works;
        }
    }
}