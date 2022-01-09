using System.Data;
using devrating.entity;
using Microsoft.Data.Sqlite;

namespace devrating.sqlite;

internal sealed class SqliteContainsAuthorOperation : ContainsAuthorOperation
{
    private readonly IDbConnection _connection;

    public SqliteContainsAuthorOperation(IDbConnection connection)
    {
        _connection = connection;
    }

    public bool Contains(string organization, string repository, string email)
    {
        using var command = _connection.CreateCommand();

        command.CommandText = @"
                SELECT Id
                FROM Author
                WHERE Email = @Email
                AND Organization = @Organization
                AND Repository = @Repository";

        command.Parameters.Add(new SqliteParameter("@Email", SqliteType.Text) { Value = email });
        command.Parameters.Add(new SqliteParameter("@Organization", SqliteType.Text) { Value = organization });
        command.Parameters.Add(new SqliteParameter("@Repository", SqliteType.Text) { Value = repository });

        using var reader = command.ExecuteReader();

        return reader.Read();
    }

    public bool Contains(Id id)
    {
        using var command = _connection.CreateCommand();

        command.CommandText = "SELECT Id FROM Author WHERE Id = @Id";

        command.Parameters.Add(new SqliteParameter("@Id", SqliteType.Integer) { Value = id.Value() });

        using var reader = command.ExecuteReader();

        return reader.Read();
    }
}
