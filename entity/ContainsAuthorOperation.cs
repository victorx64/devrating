namespace devrating.entity;

public interface ContainsAuthorOperation
{
    bool Contains(string organization, string repository, string email);
    bool Contains(Id id);
}
