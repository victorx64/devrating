// Copyright (c) 2019-present Victor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using DevRating.Domain;

namespace DevRating.DefaultObject.Fake
{
    public sealed class FakeWorks : Works
    {
        private readonly InsertWorkOperation _insert;
        private readonly GetWorkOperation _get;
        private readonly ContainsWorkOperation _contains;

        public FakeWorks(IList<Rating> ratings, IList<Work> works, IList<Author> authors)
            : this(
                new FakeInsertWorkOperation(works, ratings, authors),
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