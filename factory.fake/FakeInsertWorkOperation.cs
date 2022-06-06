using devrating.entity;

namespace devrating.factory.fake;

public sealed class FakeInsertWorkOperation : InsertWorkOperation
{
    private readonly IList<Work> _works;
    private readonly IList<Rating> _ratings;
    private readonly IList<Author> _authors;

    public FakeInsertWorkOperation(IList<Work> works, IList<Rating> ratings, IList<Author> authors)
    {
        _works = works;
        _ratings = ratings;
        _authors = authors;
    }

    public Work Insert(
        string commit,
        string? since,
        Id author,
        Id rating,
        string? link,
        DateTimeOffset createdAt
    )
    {
        var work = new FakeWork(
            new DefaultId(Guid.NewGuid()),
            Author(author),
            Rating(rating),
            commit,
            since,
            createdAt,
            link
        );

        _works.Add(work);

        return work;
    }

    private Rating Rating(Id id)
    {
        if (!id.Filled())
        {
            return new NullRating();
        }

        bool Predicate(Entity e)
        {
            return e.Id().Value().Equals(id.Value());
        }

        return _ratings.Single(Predicate);
    }

    private Author Author(Id id)
    {
        bool Predicate(Entity e)
        {
            return e.Id().Value().Equals(id.Value());
        }

        return _authors.Single(Predicate);
    }
}
