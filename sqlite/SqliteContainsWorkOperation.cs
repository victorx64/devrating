using System.Data;
using devrating.entity;
using Microsoft.Data.Sqlite;

namespace devrating.sqlite;

internal sealed class SqliteContainsWorkOperation : ContainsWorkOperation
{
    private readonly IDbConnection _connection;

    public SqliteContainsWorkOperation(IDbConnection connection)
    {
        _connection = connection;
    }

    public bool Contains(string organization, string repository, string start, string end)
    {
        using var command = _connection.CreateCommand();

        command.CommandText = @"
                SELECT w.Id
                FROM Work w
                INNER JOIN Author a on a.Id = w.AuthorId
                WHERE a.Organization = @Organization
                AND a.Repository = @Repository
                AND w.StartCommit = @StartCommit
                AND w.EndCommit = @EndCommit";

        command.Parameters.Add(new SqliteParameter("@Organization", SqliteType.Text) { Value = organization });
        command.Parameters.Add(new SqliteParameter("@Repository", SqliteType.Text) { Value = repository });
        command.Parameters.Add(new SqliteParameter("@StartCommit", SqliteType.Text, 50) { Value = start });
        command.Parameters.Add(new SqliteParameter("@EndCommit", SqliteType.Text, 50) { Value = end });

        using var reader = command.ExecuteReader();

        return reader.Read();
    }

    public bool Contains(Id id)
    {
        using var command = _connection.CreateCommand();

        command.CommandText = "SELECT Id FROM Work WHERE Id = @Id";

        command.Parameters.Add(new SqliteParameter("@Id", SqliteType.Integer) { Value = id.Value() });

        using var reader = command.ExecuteReader();

        return reader.Read();
    }

    public bool Contains(string organization, string repository, DateTimeOffset after)
    {
        using var command = _connection.CreateCommand();

        command.CommandText = @"
                SELECT w.Id
                FROM Work w
                INNER JOIN Author a on a.Id = w.AuthorId
                WHERE a.Organization = @Organization
                AND a.Repository = @Repository
                AND w.CreatedAt >= @After";

        command.Parameters.Add(new SqliteParameter("@Organization", SqliteType.Text) { Value = organization });
        command.Parameters.Add(new SqliteParameter("@Repository", SqliteType.Text) { Value = repository });
        command.Parameters.Add(new SqliteParameter("@After", SqliteType.Integer) { Value = after });

        using var reader = command.ExecuteReader();

        return reader.Read();
    }
}
