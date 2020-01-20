using System.Collections.Generic;

namespace DevRating.Domain
{
    public interface GetRatingOperation
    {
        Rating RatingOf(Entity author);
        Rating Rating(object id);
        IEnumerable<Rating> RatingsOf(Entity work);
    }
}