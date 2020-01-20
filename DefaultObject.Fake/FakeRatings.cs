using System.Collections.Generic;
using DevRating.Domain;

namespace DevRating.DefaultObject.Fake
{
    public sealed class FakeRatings : Ratings
    {
        private readonly InsertRatingOperation _insert;
        private readonly GetRatingOperation _get;
        private readonly ContainsRatingOperation _contains;

        public FakeRatings(IList<Work> works, IList<Author> authors, IList<Rating> ratings)
            : this(
                new FakeInsertRatingOperation(works, authors, ratings),
                new FakeGetRatingOperation(ratings),
                new FakeContainsRatingOperation(ratings))
        {
        }
        public FakeRatings(InsertRatingOperation insert, GetRatingOperation get, ContainsRatingOperation contains)
        {
            _insert = insert;
            _get = get;
            _contains = contains;
        }

        public InsertRatingOperation InsertOperation()
        {
            return _insert;
        }

        public GetRatingOperation GetOperation()
        {
            return _get;
        }

        public ContainsRatingOperation ContainsOperation()
        {
            return _contains;
        }
    }
}