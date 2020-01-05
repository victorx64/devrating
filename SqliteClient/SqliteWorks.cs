using System.Collections.Generic;
using System.Data;
using DevRating.Domain;
using Microsoft.Data.Sqlite;

namespace DevRating.SqliteClient
{
    internal sealed class SqliteWorks : Works
    {
        private readonly IDbConnection _connection;

        public SqliteWorks(IDbConnection connection)
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

        public bool Contains(string repository, string start, string end)
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

            return reader.Read();
        }

        public bool Contains(object id)
        {
            using var command = _connection.CreateCommand();

            command.CommandText = "SELECT Id FROM Work WHERE Id = @Id";

            command.Parameters.Add(new SqliteParameter("@Id", SqliteType.Integer) {Value = id});

            using var reader = command.ExecuteReader();

            return reader.Read();
        }

        public IEnumerable<Work> LastWorks(string repository)
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

        public Work Insert(string repository, string start, string end, Entity author, uint additions,
            Entity rating)
        {
            using var command = _connection.CreateCommand();

            command.CommandText = @"
                INSERT INTO Work
                    (Repository
                    ,Link
                    ,StartCommit
                    ,EndCommit
                    ,AuthorId
                    ,Additions
                    ,UsedRatingId)
                VALUES
                    (@Repository
                    ,NULL
                    ,@StartCommit
                    ,@EndCommit
                    ,@AuthorId
                    ,@Additions
                    ,@UsedRatingId);
                SELECT last_insert_rowid();";

            command.Parameters.Add(new SqliteParameter("@Repository", SqliteType.Text) {Value = repository});
            command.Parameters.Add(new SqliteParameter("@StartCommit", SqliteType.Text, 50) {Value = start});
            command.Parameters.Add(new SqliteParameter("@EndCommit", SqliteType.Text, 50) {Value = end});
            command.Parameters.Add(new SqliteParameter("@AuthorId", SqliteType.Integer) {Value = author.Id()});
            command.Parameters.Add(new SqliteParameter("@Additions", SqlDbType.Real) {Value = additions});
            command.Parameters.Add(new SqliteParameter("@UsedRatingId", SqliteType.Integer) {Value = rating.Id()});

            var id = command.ExecuteScalar();

            return new SqliteWork(_connection, id);
        }

        public Work Insert(string repository, string start, string end, Entity author, uint additions)
        {
            using var command = _connection.CreateCommand();

            command.CommandText = @"
                INSERT INTO Work
                    (Repository
                    ,Link
                    ,StartCommit
                    ,EndCommit
                    ,AuthorId
                    ,Additions
                    ,UsedRatingId)
                VALUES
                    (@Repository
                    ,NULL
                    ,@StartCommit
                    ,@EndCommit
                    ,@AuthorId
                    ,@Additions
                    ,NULL);
                SELECT last_insert_rowid();";

            command.Parameters.Add(new SqliteParameter("@Repository", SqliteType.Text) {Value = repository});
            command.Parameters.Add(new SqliteParameter("@StartCommit", SqliteType.Text, 50) {Value = start});
            command.Parameters.Add(new SqliteParameter("@EndCommit", SqliteType.Text, 50) {Value = end});
            command.Parameters.Add(new SqliteParameter("@AuthorId", SqliteType.Integer) {Value = author.Id()});
            command.Parameters.Add(new SqliteParameter("@Additions", SqlDbType.Real) {Value = additions});

            var id = command.ExecuteScalar();

            return new SqliteWork(_connection, id);
        }

        public Work Insert(string repository, string start, string end, Entity author, uint additions, string link)
        {
            using var command = _connection.CreateCommand();

            command.CommandText = @"
                INSERT INTO Work
                    (Repository
                    ,Link
                    ,StartCommit
                    ,EndCommit
                    ,AuthorId
                    ,Additions
                    ,UsedRatingId)
                VALUES
                    (@Repository
                    ,@Link
                    ,@StartCommit
                    ,@EndCommit
                    ,@AuthorId
                    ,@Additions
                    ,NULL);
                SELECT last_insert_rowid();";

            command.Parameters.Add(new SqliteParameter("@Repository", SqliteType.Text) {Value = repository});
            command.Parameters.Add(new SqliteParameter("@Link", SqliteType.Text) {Value = link});
            command.Parameters.Add(new SqliteParameter("@StartCommit", SqliteType.Text, 50) {Value = start});
            command.Parameters.Add(new SqliteParameter("@EndCommit", SqliteType.Text, 50) {Value = end});
            command.Parameters.Add(new SqliteParameter("@AuthorId", SqliteType.Integer) {Value = author.Id()});
            command.Parameters.Add(new SqliteParameter("@Additions", SqlDbType.Real) {Value = additions});

            var id = command.ExecuteScalar();

            return new SqliteWork(_connection, id);
        }

        public Work Insert(string repository, string start, string end, Entity author, uint additions, Entity rating, 
            string link)
        {
            using var command = _connection.CreateCommand();

            command.CommandText = @"
                INSERT INTO Work
                    (Repository
                    ,Link
                    ,StartCommit
                    ,EndCommit
                    ,AuthorId
                    ,Additions
                    ,UsedRatingId)
                VALUES
                    (@Repository
                    ,@Link
                    ,@StartCommit
                    ,@EndCommit
                    ,@AuthorId
                    ,@Additions
                    ,@UsedRatingId);
                SELECT last_insert_rowid();";

            command.Parameters.Add(new SqliteParameter("@Repository", SqliteType.Text) {Value = repository});
            command.Parameters.Add(new SqliteParameter("@Link", SqliteType.Text) {Value = link});
            command.Parameters.Add(new SqliteParameter("@StartCommit", SqliteType.Text, 50) {Value = start});
            command.Parameters.Add(new SqliteParameter("@EndCommit", SqliteType.Text, 50) {Value = end});
            command.Parameters.Add(new SqliteParameter("@AuthorId", SqliteType.Integer) {Value = author.Id()});
            command.Parameters.Add(new SqliteParameter("@Additions", SqlDbType.Real) {Value = additions});
            command.Parameters.Add(new SqliteParameter("@UsedRatingId", SqliteType.Integer) {Value = rating.Id()});

            var id = command.ExecuteScalar();

            return new SqliteWork(_connection, id);
        }
    }
}