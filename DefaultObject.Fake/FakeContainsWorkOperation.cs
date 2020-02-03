using System;
using System.Collections.Generic;
using System.Linq;
using DevRating.Domain;

namespace DevRating.DefaultObject.Fake
{
    public sealed class FakeContainsWorkOperation : ContainsWorkOperation
    {
        private readonly IList<Work> _works;

        public FakeContainsWorkOperation(IList<Work> works)
        {
            _works = works;
        }

        public bool Contains(string repository, string start, string end)
        {
            bool Predicate(Work work)
            {
                return work.Repository().Equals(repository, StringComparison.OrdinalIgnoreCase) &&
                       work.Start().Equals(start, StringComparison.OrdinalIgnoreCase) &&
                       work.End().Equals(end, StringComparison.OrdinalIgnoreCase);
            }

            return _works.Any(Predicate);
        }

        public bool Contains(Id id)
        {
            bool Predicate(Entity a)
            {
                return a.Id().Value().Equals(id.Value());
            }

            return _works.Any(Predicate);
        }
    }
}