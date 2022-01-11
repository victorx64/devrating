using System.Data;
using devrating.entity;
using Microsoft.Data.Sqlite;

namespace devrating.sqlite;

internal sealed class SqliteContainsRatingOperation : ContainsRatingOperation
{
    private readonly IDbConnection _connection;

    public SqliteContainsRatingOperation(IDbConnection connection)
    {
        _connection = connection;
    }

    public bool Contains(Id id)
    {
        using var command = _connection.CreateCommand();

        command.CommandText = "SELECT Id FROM Rating WHERE Id = @Id";

        command.Parameters.Add(new SqliteParameter("@Id", SqliteType.Integer) { Value = id.Value() });

        using var reader = command.ExecuteReader();

        return reader.Read();
    }

    public bool ContainsRatingOf(Id author)
    {
        using var command = _connection.CreateCommand();

        command.CommandText =
            "SELECT Id FROM Rating WHERE AuthorId = @AuthorId ORDER BY Id DESC LIMIT 1";

        command.Parameters.Add(new SqliteParameter("@AuthorId", SqliteType.Integer) { Value = author.Value() });

        using var reader = command.ExecuteReader();

        return reader.Read();
    }
}
