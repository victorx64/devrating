using System.Collections.Generic;

namespace DevRating.Domain.Fake
{
    public sealed class FakeWorks : Works
    {
        private readonly InsertWorkOperation _insert;
        private readonly GetWorkOperation _get;
        private readonly ContainsWorkOperation _contains;

        public FakeWorks(IList<Work> works, IList<Author> authors, IList<Rating> ratings)
            : this(
                new FakeInsertWorkOperation(works, authors, ratings),
                new FakeGetWorkOperation(works),
                new FakeContainsWorkOperation(works))
        {
        }

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