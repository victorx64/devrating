using System;
using System.Collections.Generic;
using System.Linq;

namespace DevRating.Domain.Fake
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
            throw new NotImplementedException();
        }

        public bool Contains(object id)
        {
            bool Predicate(Entity a)
            {
                return a.Id().Equals(id);
            }

            return _works.Any(Predicate);
        }
    }
}