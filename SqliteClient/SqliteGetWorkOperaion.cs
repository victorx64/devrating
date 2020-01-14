using System.Collections.Generic;
using System.Data;
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

            return new SqliteWork(_connection, reader["Id"]);
        }

        public Work Work(object id)
        {
            return new SqliteWork(_connection, id);
        }

        public IEnumerable<Work> Lasts(string repository)
        {
            using var command = _connection.CreateCommand();

            command.CommandText = @"
                SELECT w.Id
                FROM Work w
                WHERE w.Repository = @Repository
                ORDER BY w.Id DESC
                LIMIT 10";

            command.Parameters.Add(new SqliteParameter("@Repository", SqliteType.Text) {Value = repository});

            using var reader = command.ExecuteReader();

            var works = new List<SqliteWork>();

            while (reader.Read())
            {
                works.Add(new SqliteWork(_connection, reader["Id"]));
            }

            return works;
        }
    }
}