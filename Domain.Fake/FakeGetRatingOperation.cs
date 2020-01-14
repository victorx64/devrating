using System.Collections.Generic;
using System.Linq;

namespace DevRating.Domain.Fake
{
    public sealed class FakeGetRatingOperation : GetRatingOperation
    {
        private readonly IList<Rating> _ratings;

        public FakeGetRatingOperation(IList<Rating> ratings)
        {
            _ratings = ratings;
        }

        public Rating RatingOf(Entity author)
        {
            bool Predicate(Rating a)
            {
                return a.Author().Id().Equals(author.Id());
            }

            return _ratings.Last(Predicate);
        }

        public Rating Rating(object id)
        {
            return (Rating) Entity(_ratings, id);
        }

        private Entity Entity(IEnumerable<Entity> entities, object id)
        {
            bool Predicate(Entity a)
            {
                return a.Id().Equals(id);
            }

            return entities.Single(Predicate);
        }
    }
}