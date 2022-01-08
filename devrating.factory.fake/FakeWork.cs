using devrating.entity;

namespace devrating.factory.fake;

public sealed class FakeWork : Work
{
    private readonly Id _id;
    private readonly Author _author;
    private readonly Rating _rating;
    private readonly string _start;
    private readonly string _end;
    private readonly string? _since;
    private readonly string? _link;
    private readonly DateTimeOffset _createdAt;

    public FakeWork(Author author)
        : this(
            author,
            "startCommit",
            "endCommit",
            "sinceCommit",
            DateTimeOffset.UtcNow,
            "link"
        )
    {
    }

    public FakeWork(
        Author author,
        string start,
        string end,
        string? since,
        DateTimeOffset createdAt,
        string? link
    )
        : this(
            author,
            new NullRating(),
            start,
            end,
            since,
            createdAt,
            link
        )
    {
    }

    public FakeWork(
        Author author,
        Rating rating,
        string start,
        string end,
        string? since,
        DateTimeOffset createdAt,
        string? link
    )
        : this(
            new DefaultId(Guid.NewGuid()),
            author,
            rating,
            start,
            end,
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
        string start,
        string end,
        string? since,
        DateTimeOffset createdAt,
        string? link
    )
    {
        _id = id;
        _author = author;
        _rating = rating;
        _start = start;
        _end = end;
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

    public string Start()
    {
        return _start;
    }

    public string End()
    {
        return _end;
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
