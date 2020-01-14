namespace DevRating.Domain.Fake
{
    public sealed class FakeRatings : Ratings
    {
        private readonly InsertRatingOperation _insert;
        private readonly GetRatingOperation _get;
        private readonly ContainsRatingOperation _contains;

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