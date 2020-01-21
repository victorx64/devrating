using System.Collections.Generic;

namespace DevRating.Domain
{
    public interface GetRatingOperation
    {
        Rating RatingOf(Id author);
        Rating Rating(Id id);
        IEnumerable<Rating> RatingsOf(Id work);
    }
}