using devrating.entity;

namespace devrating.factory.fake;

public sealed class FakeContainsRatingOperation : ContainsRatingOperation
{
    private readonly IList<Rating> _ratings;

    public FakeContainsRatingOperation(IList<Rating> ratings)
    {
        _ratings = ratings;
    }

    public bool ContainsRatingOf(Id author)
    {
        bool RatingOfAuthor(Rating r)
        {
            return r.Author().Id().Value().Equals(author.Value());
        }

        return _ratings.Any(RatingOfAuthor);
    }

    public bool Contains(Id id)
    {
        bool Rating(Rating r)
        {
            return r.Id().Value().Equals(id.Value());
        }

        return _ratings.Any(Rating);
    }
}
