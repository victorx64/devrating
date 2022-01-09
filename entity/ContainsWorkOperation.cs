namespace devrating.entity;

public interface ContainsWorkOperation
{
    bool Contains(string organization, string repository, string start, string end);
    bool Contains(string organization, string repository, DateTimeOffset after);
    bool Contains(Id id);
}
