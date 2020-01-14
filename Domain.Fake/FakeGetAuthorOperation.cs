using System.Collections.Generic;

namespace DevRating.Domain.Fake
{
    public sealed class FakeGetAuthorOperation : GetAuthorOperation
    {
        private readonly Author _author;

        public FakeGetAuthorOperation(Author author)
        {
            _author = author;
        }

        public Author Author(string email)
        {
            return _author;
        }

        public Author Author(object id)
        {
            return _author;
        }

        public IEnumerable<Author> Top()
        {
            return new[] {_author};
        }

        public IEnumerable<Author> Top(string repository)
        {
            return Top();
        }
    }
}