// Copyright (c) 2019-present Viktor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

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

        public Author Author(string organization, string email)
        {
            bool Predicate(Author a)
            {
                return a.Organization().Equals(organization, StringComparison.OrdinalIgnoreCase) &&
                       a.Email().Equals(email, StringComparison.OrdinalIgnoreCase);
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

        public IEnumerable<Author> TopOfOrganization(string organization, DateTimeOffset after)
        {
            return _authors;
        }

        public IEnumerable<Author> TopOfRepository(string repository, DateTimeOffset after)
        {
            return _authors;
        }
    }
}