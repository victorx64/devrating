using System.Data;
using devrating.entity;

namespace devrating.sqlite;

internal sealed class SqliteAuthors : Authors
{
    private readonly GetAuthorOperation _get;
    private readonly InsertAuthorOperation _insert;
    private readonly ContainsAuthorOperation _contains;

    public SqliteAuthors(IDbConnection connection)
        : this(new SqliteGetAuthorOperation(connection),
            new SqliteInsertAuthorOperation(connection),
            new SqliteContainsAuthorOperation(connection))
    {
    }

    public SqliteAuthors(GetAuthorOperation get, InsertAuthorOperation insert, ContainsAuthorOperation contains)
    {
        _get = get;
        _insert = insert;
        _contains = contains;
    }

    public GetAuthorOperation GetOperation()
    {
        return _get;
    }

    public InsertAuthorOperation InsertOperation()
    {
        return _insert;
    }

    public ContainsAuthorOperation ContainsOperation()
    {
        return _contains;
    }
}
