using System.Data;
using devrating.entity;

namespace devrating.sqlite;

internal sealed class SqliteEntities : Entities
{
    private readonly Works _works;
    private readonly Ratings _ratings;
    private readonly Authors _authors;

    public SqliteEntities(IDbConnection connection)
        : this(new SqliteWorks(connection),
            new SqliteRatings(connection),
            new SqliteAuthors(connection))
    {
    }

    public SqliteEntities(Works works, Ratings ratings, Authors authors)
    {
        _works = works;
        _ratings = ratings;
        _authors = authors;
    }

    public Works Works()
    {
        return _works;
    }

    public Ratings Ratings()
    {
        return _ratings;
    }

    public Authors Authors()
    {
        return _authors;
    }
}
