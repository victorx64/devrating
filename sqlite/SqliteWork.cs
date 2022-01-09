using System.Data;
using System.Globalization;
using devrating.factory;
using devrating.entity;
using Microsoft.Data.Sqlite;

namespace devrating.sqlite;

internal sealed class SqliteWork : Work
{
    private readonly IDbConnection _connection;
    private readonly Id _id;

    public SqliteWork(IDbConnection connection, Id id)
    {
        _connection = connection;
        _id = id;
    }

    public Id Id()
    {
        return _id;
    }

    public Author Author()
    {
        using var command = _connection.CreateCommand();

        command.CommandText = "SELECT AuthorId FROM Work WHERE Id = @Id";

        command.Parameters.Add(new SqliteParameter("@Id", SqliteType.Integer) { Value = _id.Value() });

        using var reader = command.ExecuteReader();

        reader.Read();

        return new SqliteAuthor(_connection, new DefaultId(reader["AuthorId"]));
    }

    public Rating UsedRating()
    {
        using var command = _connection.CreateCommand();

        command.CommandText = "SELECT UsedRatingId FROM Work WHERE Id = @Id";

        command.Parameters.Add(new SqliteParameter("@Id", SqliteType.Integer) { Value = _id.Value() });

        using var reader = command.ExecuteReader();

        reader.Read();

        return new SqliteRating(_connection, new DefaultId(reader["UsedRatingId"]));
    }

    public string Start()
    {
        using var command = _connection.CreateCommand();

        command.CommandText = "SELECT StartCommit FROM Work WHERE Id = @Id";

        command.Parameters.Add(new SqliteParameter("@Id", SqliteType.Integer) { Value = _id.Value() });

        return (string)command.ExecuteScalar()!;
    }

    public string End()
    {
        using var command = _connection.CreateCommand();

        command.CommandText = "SELECT EndCommit FROM Work WHERE Id = @Id";

        command.Parameters.Add(new SqliteParameter("@Id", SqliteType.Integer) { Value = _id.Value() });

        return (string)command.ExecuteScalar()!;
    }

    public string? Since()
    {
        using var command = _connection.CreateCommand();

        command.CommandText = "SELECT SinceCommit FROM Work WHERE Id = @Id";

        command.Parameters.Add(new SqliteParameter("@Id", SqliteType.Integer) { Value = _id.Value() });

        return command.ExecuteScalar() as string;
    }

    public DateTimeOffset CreatedAt()
    {
        using var command = _connection.CreateCommand();

        command.CommandText = "SELECT CreatedAt FROM Work WHERE Id = @Id";

        command.Parameters.Add(new SqliteParameter("@Id", SqliteType.Integer) { Value = _id.Value() });

        return DateTimeOffset.Parse(command.ExecuteScalar()!.ToString()!, CultureInfo.InvariantCulture);
    }

    public string? Link()
    {
        using var command = _connection.CreateCommand();

        command.CommandText = "SELECT Link FROM Work WHERE Id = @Id";

        command.Parameters.Add(new SqliteParameter("@Id", SqliteType.Integer) { Value = _id.Value() });

        return command.ExecuteScalar() as string;
    }
}
