using System.Collections.Generic;
using System.Linq;

namespace DevRating.Domain.Fake
{
    public sealed class FakeContainsRatingOperation : ContainsRatingOperation
    {
        private readonly IList<Rating> _ratings;

        public FakeContainsRatingOperation(IList<Rating> ratings)
        {
            _ratings = ratings;
        }

        public bool ContainsRatingOf(Entity author)
        {
            bool RatingOfAuthor(Rating r)
            {
                return r.Author().Id().Equals(author.Id());
            }

            return _ratings.Any(RatingOfAuthor);
        }

        public bool Contains(object id)
        {
            bool Rating(Rating r)
            {
                return r.Id().Equals(id);
            }

            return _ratings.Any(Rating);
        }
    }
}