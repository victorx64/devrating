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
        string start,
        string end,
        string? since,
        string email,
        string? link,
        DateTimeOffset createdAt
    )
    {
        if (_works.ContainsOperation().Contains(organization, repository, createdAt))
        {
            throw new InvalidOperationException("An older Work is already exist for this repo");
        }

        if (_works.ContainsOperation().Contains(organization, repository, start, end))
        {
            throw new InvalidOperationException("The Work is already present");
        }

        var author = _authorFactory.AuthorAtOrg(organization, repository, email, createdAt);

        return _works.InsertOperation().Insert(
            start,
            end,
            since,
            author,
            _ratings.GetOperation().RatingOf(author).Id(),
            link,
            createdAt
        );
    }
}