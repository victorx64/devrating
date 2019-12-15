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

        public Work Insert(string repository, string start, string end, IdObject author, uint additions,
            IdObject rating)
        {
            using var command = _connection.CreateCommand();

            command.CommandText = @"
                INSERT INTO Work
                    (Repository
                    ,StartCommit
                    ,EndCommit
                    ,AuthorId
                    ,Additions
                    ,UsedRatingId)
                VALUES
                    (@Repository
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

        public Work Insert(string repository, string start, string end, IdObject author, uint additions)
        {
            using var command = _connection.CreateCommand();

            command.CommandText = @"
                INSERT INTO Work
                    (Repository
                    ,StartCommit
                    ,EndCommit
                    ,AuthorId
                    ,Additions
                    ,UsedRatingId)
                VALUES
                    (@Repository
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
    }
}