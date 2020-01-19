using System.Data;
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

        public Rating Insert(double value, DbParameter deletions, Entity previous, Entity work, Entity author)
        {
            using var command = _connection.CreateCommand();

            command.CommandText = @"
                INSERT INTO Rating
                    (Rating
                    ,Deletions
                    ,PreviousRatingId
                    ,WorkId
                    ,AuthorId)
                VALUES
                    (@Rating
                    ,@Deletions
                    ,@PreviousRatingId
                    ,@WorkId
                    ,@AuthorId);
                SELECT last_insert_rowid();";

            command.Parameters.Add(new SqliteParameter("@Rating", SqliteType.Real) {Value = value});
            command.Parameters.Add(new SqliteParameter("@PreviousRatingId", SqliteType.Integer) {Value = previous.Id()});
            command.Parameters.Add(new SqliteParameter("@WorkId", SqliteType.Integer) {Value = work.Id()});
            command.Parameters.Add(new SqliteParameter("@AuthorId", SqliteType.Integer) {Value = author.Id()});
            command.Parameters.Add(new SqliteParameter("@Deletions", SqliteType.Integer) {Value = deletions.Value()});

            return new SqliteRating(_connection, command.ExecuteScalar());
        }
    }
}