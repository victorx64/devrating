using devrating.entity;

namespace devrating.factory;

public sealed class DefaultAuthorFactory : AuthorFactory
{
    private readonly Authors _authors;

    public DefaultAuthorFactory(Authors authors)
    {
        _authors = authors;
    }

    public Id AuthorAtOrg(
        string organization,
        string repository,
        string email,
        DateTimeOffset createdAt
    )
    {
        return _authors.ContainsOperation().Contains(organization, repository, email)
            ? _authors.GetOperation().Author(organization, repository, email).Id()
            : _authors.InsertOperation().Insert(organization, repository, email, createdAt).Id();
    }
}
