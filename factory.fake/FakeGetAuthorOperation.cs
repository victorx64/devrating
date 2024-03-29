using devrating.entity;

namespace devrating.factory.fake;

public sealed class FakeGetAuthorOperation : GetAuthorOperation
{
    private readonly IList<Author> _authors;

    public FakeGetAuthorOperation(IList<Author> authors)
    {
        _authors = authors;
    }

    public Author Author(string organization, string repository, string email)
    {
        bool Predicate(Author a)
        {
            return a.Organization().Equals(organization, StringComparison.OrdinalIgnoreCase) &&
                a.Repository().Equals(repository, StringComparison.OrdinalIgnoreCase) &&
                a.Email().Equals(email, StringComparison.OrdinalIgnoreCase);
        }

        return _authors.Single(Predicate);
    }

    public Author Author(Id id)
    {
        bool Predicate(Author a)
        {
            return a.Id().Value().Equals(id.Value());
        }

        return _authors.Single(Predicate);
    }

    public IEnumerable<Author> Top(string organization, string repository, DateTimeOffset after)
    {
        return _authors;
    }
}
