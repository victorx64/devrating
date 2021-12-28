// Copyright (c) 2019-present Victor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using DevRating.Domain;

namespace DevRating.DefaultObject.Fake
{
    public sealed class FakeAuthors : Authors
    {
        private readonly GetAuthorOperation _get;
        private readonly InsertAuthorOperation _insert;
        private readonly ContainsAuthorOperation _contains;

        public FakeAuthors(IList<Author> authors)
            : this(
                new FakeGetAuthorOperation(authors), 
                new FakeInsertAuthorOperation(authors),
                new FakeContainsAuthorOperation(authors))
        {
        }

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