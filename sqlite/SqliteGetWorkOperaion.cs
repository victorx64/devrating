using System.Data;
using devrating.factory;
using devrating.entity;
using Microsoft.Data.Sqlite;

namespace devrating.sqlite;

internal sealed class SqliteGetWorkOperation : GetWorkOperation
{
    private readonly IDbConnection _connection;

    public SqliteGetWorkOperation(IDbConnection connection)
    {
        _connection = connection;
    }

    public Work Work(string organization, string repository, string commit)
    {
        using var command = _connection.CreateCommand();

        command.CommandText = @"
                SELECT w.Id 
                FROM Work w
                INNER JOIN Author a on a.Id = w.AuthorId
                WHERE a.Organization = @Organization
                AND a.Repository = @Repository
                AND w.MergeCommit = @MergeCommit";

        command.Parameters.Add(new SqliteParameter("@Organization", SqliteType.Text) { Value = organization });
        command.Parameters.Add(new SqliteParameter("@Repository", SqliteType.Text) { Value = repository });
        command.Parameters.Add(new SqliteParameter("@MergeCommit", SqliteType.Text, 50) { Value = commit });

        using var reader = command.ExecuteReader();

        reader.Read();

        return new SqliteWork(_connection, new DefaultId(reader["Id"]));
    }

    public Work Work(Id id)
    {
        return new SqliteWork(_connection, id);
    }

    public IEnumerable<Work> Last(string organization, string repository, DateTimeOffset after)
    {
        using var command = _connection.CreateCommand();

        command.CommandText = @"
                SELECT w.Id
                FROM Work w
                INNER JOIN Author a on a.Id = w.AuthorId
                WHERE a.Organization = @Organization AND a.Repository = @Repository AND w.CreatedAt >= @After
                ORDER BY w.Id DESC";

        command.Parameters.Add(new SqliteParameter("@Organization", SqliteType.Text) { Value = organization });
        command.Parameters.Add(new SqliteParameter("@Repository", SqliteType.Text) { Value = repository });
        command.Parameters.Add(new SqliteParameter("@After", SqliteType.Integer) { Value = after });

        using var reader = command.ExecuteReader();

        var works = new List<SqliteWork>();

        while (reader.Read())
        {
            works.Add(new SqliteWork(_connection, new DefaultId(reader["Id"])));
        }

        return works;
    }

    public IEnumerable<Work> Last(Id author, DateTimeOffset after)
    {
        using var command = _connection.CreateCommand();

        command.CommandText = @"
                SELECT w.Id
                FROM Work w
                WHERE w.AuthorId = @AuthorId AND w.CreatedAt >= @After
                ORDER BY w.Id DESC";

        command.Parameters.Add(new SqliteParameter("@AuthorId", SqliteType.Integer) { Value = author.Value() });
        command.Parameters.Add(new SqliteParameter("@After", SqliteType.Integer) { Value = after });

        using var reader = command.ExecuteReader();

        var works = new List<Work>();

        while (reader.Read())
        {
            works.Add(new SqliteWork(_connection, new DefaultId(reader["Id"])));
        }

        return works;
    }
}
