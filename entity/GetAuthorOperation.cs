namespace devrating.entity;

public interface GetAuthorOperation
{
    Author Author(string organization, string repository, string email);
    Author Author(Id id);
    IEnumerable<Author> Top(string organization, string repository, DateTimeOffset after);
}
