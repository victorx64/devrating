using System;
using System.Collections.Generic;
using System.Linq;

namespace DevRating.Domain.Fake
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

        public Work Work(object id)
        {
            return Entity(_works, id) as Work;
        }

        public IEnumerable<Work> Lasts(string repository)
        {
            return _works;
        }

        private Entity Entity(IEnumerable<Entity> entities, object id)
        {
            bool Predicate(Entity a)
            {
                return a.Id().Equals(id);
            }

            return entities.Single(Predicate);
        }
    }
}