using System.Data;
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

        public Work Insert(string repository, string start, string end, Entity author, uint additions,
            Entity rating, ObjectEnvelope link)
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
            command.Parameters.Add(new SqliteParameter("@Link", SqliteType.Text) {Value = link.Value()});
            command.Parameters.Add(new SqliteParameter("@StartCommit", SqliteType.Text, 50) {Value = start});
            command.Parameters.Add(new SqliteParameter("@EndCommit", SqliteType.Text, 50) {Value = end});
            command.Parameters.Add(new SqliteParameter("@AuthorId", SqliteType.Integer) {Value = author.Id()});
            command.Parameters.Add(new SqliteParameter("@Additions", SqliteType.Integer) {Value = additions});
            command.Parameters.Add(new SqliteParameter("@UsedRatingId", SqliteType.Integer) {Value = rating.Id()});

            return new SqliteWork(_connection, command.ExecuteScalar());
        }
    }
}