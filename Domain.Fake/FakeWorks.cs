namespace DevRating.Domain.Fake
{
    public sealed class FakeWorks : Works
    {
        private readonly InsertWorkOperation _insert;
        private readonly GetWorkOperation _get;
        private readonly ContainsWorkOperation _contains;

        public FakeWorks(InsertWorkOperation insert, GetWorkOperation get, ContainsWorkOperation contains)
        {
            _insert = insert;
            _get = get;
            _contains = contains;
        }

        public InsertWorkOperation InsertOperation()
        {
            return _insert;
        }

        public GetWorkOperation GetOperation()
        {
            return _get;
        }

        public ContainsWorkOperation ContainsOperation()
        {
            return _contains;
        }
    }
}