using devrating.entity;

namespace devrating.factory.fake;

public sealed class FakeContainsAuthorOperation : ContainsAuthorOperation
{
    private readonly IList<Author> _authors;

    public FakeContainsAuthorOperation(IList<Author> authors)
    {
        _authors = authors;
    }

    public bool Contains(string organization, string repository, string email)
    {
        bool Predicate(Author a)
        {
            return a.Organization().Equals(organization, StringComparison.OrdinalIgnoreCase) &&
                   a.Repository().Equals(repository, StringComparison.OrdinalIgnoreCase) &&
                   a.Email().Equals(email, StringComparison.OrdinalIgnoreCase);
        }

        return _authors.Any(Predicate);
    }

    public bool Contains(Id id)
    {
        bool Predicate(Author a)
        {
            return a.Id().Value().Equals(id.Value());
        }

        return _authors.Any(Predicate);
    }
}
