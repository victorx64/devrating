using devrating.entity;

namespace devrating.factory.fake;

public sealed class FakeWork : Work
{
    private readonly Id _id;
    private readonly Author _author;
    private readonly Rating _rating;
    private readonly string _commit;
    private readonly string? _since;
    private readonly string? _link;
    private readonly DateTimeOffset _createdAt;

    public FakeWork(Author author)
        : this(
            author,
            "mergeCommit",
            "sinceCommit",
            DateTimeOffset.UtcNow,
            "link"
        )
    {
    }

    public FakeWork(
        Author author,
        string commit,
        string? since,
        DateTimeOffset createdAt,
        string? link
    )
        : this(
            author,
            new NullRating(),
            commit,
            since,
            createdAt,
            link
        )
    {
    }

    public FakeWork(
        Author author,
        Rating rating,
        string commit,
        string? since,
        DateTimeOffset createdAt,
        string? link
    )
        : this(
            new DefaultId(Guid.NewGuid()),
            author,
            rating,
            commit,
            since,
            createdAt,
            link
        )
    {
    }

    public FakeWork(
        Id id,
        Author author,
        Rating rating,
        string commit,
        string? since,
        DateTimeOffset createdAt,
        string? link
    )
    {
        _id = id;
        _author = author;
        _rating = rating;
        _commit = commit;
        _since = since;
        _createdAt = createdAt;
        _link = link;
    }

    public Id Id()
    {
        return _id;
    }

    public string ToJson()
    {
        throw new NotImplementedException();
    }

    public Author Author()
    {
        return _author;
    }

    public Rating UsedRating()
    {
        return _rating;
    }

    public string Commit()
    {
        return _commit;
    }

    public string? Since()
    {
        return _since;
    }

    public DateTimeOffset CreatedAt()
    {
        return _createdAt;
    }

    public string? Link()
    {
        return _link;
    }
}
