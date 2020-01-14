namespace DevRating.Domain.Fake
{
    public sealed class FakeContainsAuthorOperation : ContainsAuthorOperation
    {
        private readonly bool _contains;

        public FakeContainsAuthorOperation(bool contains)
        {
            _contains = contains;
        }

        public bool Contains(string email)
        {
            return _contains;
        }

        public bool Contains(object id)
        {
            return _contains;
        }
    }
}