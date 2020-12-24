// Copyright (c) 2019-present Viktor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using DevRating.Domain;

namespace DevRating.DefaultObject.Fake
{
    public sealed class FakeGetWorkOperation : GetWorkOperation
    {
        private readonly IList<Work> _works;

        public FakeGetWorkOperation(IList<Work> works)
        {
            _works = works;
        }

        public Work Work(string repository, string start, string end)
        {
            bool Predicate(Work work)
            {
                return work.Repository().Equals(repository, StringComparison.OrdinalIgnoreCase) &&
                       work.Start().Equals(start, StringComparison.OrdinalIgnoreCase) &&
                       work.End().Equals(end, StringComparison.OrdinalIgnoreCase);
            }

            return _works.Single(Predicate);
        }

        public Work Work(Id id)
        {
            bool Predicate(Entity a)
            {
                return a.Id().Value().Equals(id.Value());
            }

            return _works.Single(Predicate);
        }

        public IEnumerable<Work> Last(string repository, DateTimeOffset after)
        {
            return _works;
        }

        public IEnumerable<Work> LastOfOrganization(string organization, DateTimeOffset after)
        {
            return _works;
        }
    }
}