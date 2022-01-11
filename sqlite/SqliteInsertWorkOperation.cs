using System.Data;
using devrating.factory;
using devrating.entity;
using Microsoft.Data.Sqlite;

namespace devrating.sqlite;

internal sealed class SqliteInsertWorkOperation : InsertWorkOperation
{
    private readonly IDbConnection _connection;

    public SqliteInsertWorkOperation(IDbConnection connection)
    {
        _connection = connection;
    }

    public Work Insert(
        string start,
        string end,
        string? since,
        Id author,
        Id rating,
        string? link,
        DateTimeOffset createdAt
    )
    {
        using var command = _connection.CreateCommand();

        command.CommandText = @"
                INSERT INTO Work
                    (Link
                    ,StartCommit
                    ,EndCommit
                    ,SinceCommit
                    ,AuthorId
                    ,UsedRatingId
                    ,CreatedAt)
                VALUES
                    (@Link
                    ,@StartCommit
                    ,@EndCommit
                    ,@SinceCommit
                    ,@AuthorId
                    ,@UsedRatingId
                    ,@CreatedAt);
                SELECT last_insert_rowid();";

        command.Parameters.Add(new SqliteParameter("@Link", SqliteType.Text) { Value = link ?? (object)DBNull.Value });
        command.Parameters.Add(new SqliteParameter("@StartCommit", SqliteType.Text, 50) { Value = start });
        command.Parameters.Add(new SqliteParameter("@EndCommit", SqliteType.Text, 50) { Value = end });
        command.Parameters.Add(new SqliteParameter("@SinceCommit", SqliteType.Text, 50) { Value = since ?? (object)DBNull.Value });
        command.Parameters.Add(new SqliteParameter("@AuthorId", SqliteType.Integer) { Value = author.Value() });
        command.Parameters.Add(new SqliteParameter("@UsedRatingId", SqliteType.Integer) { Value = rating.Value() });
        command.Parameters.Add(new SqliteParameter("@CreatedAt", SqliteType.Integer) { Value = createdAt });

        return new SqliteWork(_connection, new DefaultId(command.ExecuteScalar()!));
    }
}
