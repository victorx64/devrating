namespace DevRating.Domain.Fake
{
    public sealed class FakeAuthors : Authors
    {
        private readonly GetAuthorOperation _get;
        private readonly InsertAuthorOperation _insert;
        private readonly ContainsAuthorOperation _contains;

        public FakeAuthors(GetAuthorOperation get, InsertAuthorOperation insert, ContainsAuthorOperation contains)
        {
            _get = get;
            _insert = insert;
            _contains = contains;
        }

        public GetAuthorOperation GetOperation()
        {
            return _get;
        }

        public InsertAuthorOperation InsertOperation()
        {
            return _insert;
        }

        public ContainsAuthorOperation ContainsOperation()
        {
            return _contains;
        }
    }
}