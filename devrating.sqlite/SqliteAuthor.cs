using System.Data;
using System.Globalization;
using devrating.entity;
using Microsoft.Data.Sqlite;

namespace devrating.sqlite;

internal sealed class SqliteAuthor : Author
{
    private readonly IDbConnection _connection;
    private readonly Id _id;

    public SqliteAuthor(IDbConnection connection, Id id)
    {
        _connection = connection;
        _id = id;
    }

    public Id Id()
    {
        return _id;
    }

    public string Email()
    {
        using var command = _connection.CreateCommand();

        command.CommandText = "SELECT Email FROM Author WHERE Id = @Id";

        command.Parameters.Add(new SqliteParameter("@Id", SqliteType.Integer) { Value = _id.Value() });

        using var reader = command.ExecuteReader();

        reader.Read();

        return (string)reader["Email"];
    }

    public string Organization()
    {
        using var command = _connection.CreateCommand();

        command.CommandText = "SELECT Organization FROM Author WHERE Id = @Id";

        command.Parameters.Add(new SqliteParameter("@Id", SqliteType.Integer) { Value = _id.Value() });

        using var reader = command.ExecuteReader();

        reader.Read();

        return (string)reader["Organization"];
    }

    public string Repository()
    {
        using var command = _connection.CreateCommand();

        command.CommandText = "SELECT Repository FROM Author WHERE Id = @Id";

        command.Parameters.Add(new SqliteParameter("@Id", SqliteType.Integer) { Value = _id.Value() });

        return (string)command.ExecuteScalar()!;
    }

    public DateTimeOffset CreatedAt()
    {
        using var command = _connection.CreateCommand();

        command.CommandText = "SELECT CreatedAt FROM Author WHERE Id = @Id";

        command.Parameters.Add(new SqliteParameter("@Id", SqliteType.Integer) { Value = _id.Value() });

        return DateTimeOffset.Parse(command.ExecuteScalar()!.ToString()!, CultureInfo.InvariantCulture);
    }
}
