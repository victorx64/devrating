// Copyright (c) 2019-present Victor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using DevRating.Domain;

namespace DevRating.DefaultObject.Fake
{
    public sealed class FakeContainsAuthorOperation : ContainsAuthorOperation
    {
        private readonly IList<Author> _authors;

        public FakeContainsAuthorOperation(IList<Author> authors)
        {
            _authors = authors;
        }

        public bool Contains(string organization, string repository, string email)
        {
            bool Predicate(Author a)
            {
                return a.Organization().Equals(organization, StringComparison.OrdinalIgnoreCase) &&
                       a.Repository().Equals(repository, StringComparison.OrdinalIgnoreCase) &&
                       a.Email().Equals(email, StringComparison.OrdinalIgnoreCase);
            }

            return _authors.Any(Predicate);
        }

        public bool Contains(Id id)
        {
            bool Predicate(Author a)
            {
                return a.Id().Value().Equals(id.Value());
            }

            return _authors.Any(Predicate);
        }
    }
}