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
            throw new NotImplementedException();
        }

        public Work Work(Id id)
        {
            bool Predicate(Entity a)
            {
                return a.Id().Value().Equals(id.Value());
            }

            return _works.Single(Predicate);
        }

        public IEnumerable<Work> Lasts(string repository)
        {
            return _works;
        }
    }
}