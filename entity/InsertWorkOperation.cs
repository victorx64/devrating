namespace devrating.entity;

public interface InsertWorkOperation
{
    Work Insert(
        string start,
        string end,
        string? since,
        Id author,
        Id rating,
        string? link,
        DateTimeOffset createdAt
    );
}
