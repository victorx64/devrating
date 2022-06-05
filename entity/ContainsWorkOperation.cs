namespace devrating.entity;

public interface ContainsWorkOperation
{
    bool Contains(string organization, string repository, string commit);
    bool Contains(string organization, string repository, DateTimeOffset after);
    bool Contains(Id id);
}
