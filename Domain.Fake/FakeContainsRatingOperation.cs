namespace DevRating.Domain.Fake
{
    public sealed class FakeContainsRatingOperation : ContainsRatingOperation
    {
        private readonly bool _contains;

        public FakeContainsRatingOperation(bool contains)
        {
            _contains = contains;
        }

        public bool ContainsRatingOf(Entity author)
        {
            return _contains;
        }

        public bool Contains(object id)
        {
            return _contains;
        }
    }
}