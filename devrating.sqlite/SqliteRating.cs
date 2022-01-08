using System.Data;
using devrating.factory;
using devrating.entity;
using Microsoft.Data.Sqlite;

namespace devrating.sqlite;

internal sealed class SqliteRating : Rating
{
    private readonly IDbConnection _connection;
    private readonly Id _id;

    public SqliteRating(IDbConnection connection, Id id)
    {
        _connection = connection;
        _id = id;
    }

    public Id Id()
    {
        return _id;
    }

    public Rating PreviousRating()
    {
        using var command = _connection.CreateCommand();

        command.CommandText = "SELECT PreviousRatingId FROM Rating WHERE Id = @Id";

        command.Parameters.Add(new SqliteParameter("@Id", SqliteType.Integer) { Value = _id.Value() });

        using var reader = command.ExecuteReader();

        reader.Read();

        return new SqliteRating(_connection, new DefaultId(reader["PreviousRatingId"]));
    }

    public Work Work()
    {
        using var command = _connection.CreateCommand();

        command.CommandText = "SELECT WorkId FROM Rating WHERE Id = @Id";

        command.Parameters.Add(new SqliteParameter("@Id", SqliteType.Integer) { Value = _id.Value() });

        using var reader = command.ExecuteReader();

        reader.Read();

        return new SqliteWork(_connection, new DefaultId(reader["WorkId"]));
    }

    public Author Author()
    {
        using var command = _connection.CreateCommand();

        command.CommandText = "SELECT AuthorId FROM Rating WHERE Id = @Id";

        command.Parameters.Add(new SqliteParameter("@Id", SqliteType.Integer) { Value = _id.Value() });

        using var reader = command.ExecuteReader();

        reader.Read();

        return new SqliteAuthor(_connection, new DefaultId(reader["AuthorId"]));
    }

    public double Value()
    {
        using var command = _connection.CreateCommand();

        command.CommandText = "SELECT Rating FROM Rating WHERE Id = @Id";

        command.Parameters.Add(new SqliteParameter("@Id", SqliteType.Integer) { Value = _id.Value() });

        using var reader = command.ExecuteReader();

        reader.Read();

        return (double)reader["Rating"];
    }
}
