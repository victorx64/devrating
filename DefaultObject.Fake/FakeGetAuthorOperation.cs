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

        public Author Author(Id id)
        {
            bool Predicate(Author a)
            {
                return a.Id().Value().Equals(id.Value());
            }

            return _authors.Single(Predicate);
        }

        public IEnumerable<Author> TopOfOrganization(string organization)
        {
            return _authors;
        }

        public IEnumerable<Author> TopOfRepository(string repository)
        {
            return _authors;
        }
    }
}