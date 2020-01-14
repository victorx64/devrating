namespace DevRating.Domain.Fake
{
    public sealed class FakeContainsWorkOperation : ContainsWorkOperation
    {
        private readonly bool _contains;

        public FakeContainsWorkOperation(bool contains)
        {
            _contains = contains;
        }

        public bool Contains(string repository, string start, string end)
        {
            return _contains;
        }

        public bool Contains(object id)
        {
            return _contains;
        }
    }
}