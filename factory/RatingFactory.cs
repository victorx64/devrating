using devrating.entity;

namespace devrating.factory;

public interface RatingFactory
{
    IEnumerable<Rating> NewRatings(
        string organization,
        string repository,
        string email,
        IEnumerable<ContemporaryLines> deletions,
        Id work,
        DateTimeOffset createdAt
    );
}
