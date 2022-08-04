using devrating.entity;

namespace devrating.factory;

public sealed class DefaultWorkFactory : WorkFactory
{
    private readonly Works _works;
    private readonly Ratings _ratings;
    private readonly AuthorFactory _authorFactory;

    public DefaultWorkFactory(Works works, Ratings ratings, AuthorFactory authorFactory)
    {
        _works = works;
        _ratings = ratings;
        _authorFactory = authorFactory;
    }

    public Work NewWork(
        string organization,
        string repository,
        string commit,
        string? since,
        string email,
        string? link,
        DateTimeOffset createdAt
    )
    {
        if (_works.ContainsOperation().Contains(organization, repository, createdAt))
        {
            throw new InvalidOperationException("A newer Work already exists for this repo");
        }

        if (_works.ContainsOperation().Contains(organization, repository, commit))
        {
            throw new InvalidOperationException("The Work is already present");
        }

        var author = _authorFactory.AuthorAtOrg(organization, repository, email, createdAt);

        return _works.InsertOperation().Insert(
            commit,
            since,
            author,
            _ratings.GetOperation().RatingOf(author).Id(),
            link,
            createdAt
        );
    }
}
