using System;
using System.Collections.Generic;
using System.Linq;
using DevRating.Domain;

namespace DevRating.DefaultObject.Fake
{
    public sealed class FakeGetAuthorOperation : GetAuthorOperation
    {
        private readonly IList<Author> _authors;

        public FakeGetAuthorOperation(IList<Author> authors)
        {
            _authors = authors;
        }

        public Author Author(string email)
        {
            bool Predicate(Author a)
            {
                return a.Email().Equals(email, StringComparison.OrdinalIgnoreCase);
            }

            return _authors.Single(Predicate);
        }

        public Author Author(object id)
        {
            bool Predicate(Author a)
            {
                return a.Id().Equals(id);
            }

            return _authors.Single(Predicate);
        }

        public IEnumerable<Author> Top()
        {
            return _authors;
        }

        public IEnumerable<Author> Top(string repository)
        {
            return _authors;
        }
    }
}