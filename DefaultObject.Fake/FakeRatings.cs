using System.Collections.Generic;
using DevRating.Domain;

namespace DevRating.DefaultObject.Fake
{
    public sealed class FakeRatings : Ratings
    {
        private readonly InsertRatingOperation _insert;
        private readonly GetRatingOperation _get;
        private readonly ContainsRatingOperation _contains;

        public FakeRatings(IList<Rating> ratings, IList<Work> works, IList<Author> authors)
            : this(
                new FakeInsertRatingOperation(ratings, works, authors),
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