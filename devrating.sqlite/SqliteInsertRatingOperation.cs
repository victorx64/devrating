using System.Data;
using devrating.factory;
using devrating.entity;
using Microsoft.Data.Sqlite;

namespace devrating.sqlite;

internal sealed class SqliteInsertRatingOperation : InsertRatingOperation
{
    private readonly IDbConnection _connection;

    public SqliteInsertRatingOperation(IDbConnection connection)
    {
        _connection = connection;
    }

    public Rating Insert(
        double value,
        Id previous,
        Id work,
        Id author
    )
    {
        using var command = _connection.CreateCommand();

        command.CommandText = @"
                INSERT INTO Rating
                    (Rating
                    ,PreviousRatingId
                    ,WorkId
                    ,AuthorId)
                VALUES
                    (@Rating
                    ,@PreviousRatingId
                    ,@WorkId
                    ,@AuthorId);
                SELECT last_insert_rowid();";

        command.Parameters.Add(new SqliteParameter("@Rating", SqliteType.Real) { Value = value });
        command.Parameters.Add(new SqliteParameter("@PreviousRatingId", SqliteType.Integer)
        { Value = previous.Value() });
        command.Parameters.Add(new SqliteParameter("@WorkId", SqliteType.Integer) { Value = work.Value() });
        command.Parameters.Add(new SqliteParameter("@AuthorId", SqliteType.Integer) { Value = author.Value() });

        return new SqliteRating(_connection, new DefaultId(command.ExecuteScalar()!));
    }
}
