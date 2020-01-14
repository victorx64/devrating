using System;
using System.Collections.Generic;
using System.Linq;

namespace DevRating.Domain.Fake
{
    public sealed class FakeContainsAuthorOperation : ContainsAuthorOperation
    {
        private readonly IList<Author> _authors;

        public FakeContainsAuthorOperation(IList<Author> authors)
        {
            _authors = authors;
        }

        public bool Contains(string email)
        {
            bool Predicate(Author a)
            {
                return a.Email().Equals(email, StringComparison.OrdinalIgnoreCase);
            }

            return _authors.Any(Predicate);
        }

        public bool Contains(object id)
        {
            bool Predicate(Author a)
            {
                return a.Id().Equals(id);
            }

            return _authors.Any(Predicate);
        }
    }
}