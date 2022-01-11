namespace devrating.entity;

public interface GetRatingOperation
{
    Rating RatingOf(Id author);
    Rating Rating(Id id);
    IEnumerable<Rating> RatingsOf(Id work);
    IEnumerable<Rating> Last(Id author, DateTimeOffset after);
}
