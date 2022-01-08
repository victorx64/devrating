using System.Data;
using devrating.entity;

namespace devrating.sqlite;

internal sealed class SqliteWorks : Works
{
    private readonly InsertWorkOperation _insert;
    private readonly GetWorkOperation _get;
    private readonly ContainsWorkOperation _contains;

    public SqliteWorks(IDbConnection connection)
        : this(new SqliteInsertWorkOperation(connection),
            new SqliteGetWorkOperation(connection),
            new SqliteContainsWorkOperation(connection))
    {
    }

    public SqliteWorks(InsertWorkOperation insert, GetWorkOperation get, ContainsWorkOperation contains)
    {
        _insert = insert;
        _get = get;
        _contains = contains;
    }

    public InsertWorkOperation InsertOperation()
    {
        return _insert;
    }

    public GetWorkOperation GetOperation()
    {
        return _get;
    }

    public ContainsWorkOperation ContainsOperation()
    {
        return _contains;
    }
}
