namespace devrating.entity;

public interface InsertWorkOperation
{
    Work Insert(
        string commit,
        string? since,
        Id author,
        Id rating,
        string? link,
        DateTimeOffset createdAt
    );
}
