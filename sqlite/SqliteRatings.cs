using System.Data;
using devrating.entity;

namespace devrating.sqlite;

internal sealed class SqliteRatings : Ratings
{
    private readonly InsertRatingOperation _insert;
    private readonly GetRatingOperation _get;
    private readonly ContainsRatingOperation _contains;

    public SqliteRatings(IDbConnection connection)
        : this(new SqliteInsertRatingOperation(connection),
            new SqliteGetRatingOperation(connection),
            new SqliteContainsRatingOperation(connection))
    {
    }

    public SqliteRatings(InsertRatingOperation insert, GetRatingOperation get, ContainsRatingOperation contains)
    {
        _insert = insert;
        _get = get;
        _contains = contains;
    }

    public InsertRatingOperation InsertOperation()
    {
        return _insert;
    }

    public GetRatingOperation GetOperation()
    {
        return _get;
    }

    public ContainsRatingOperation ContainsOperation()
    {
        return _contains;
    }
}
