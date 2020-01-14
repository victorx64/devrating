using System.Collections.Generic;

namespace DevRating.Domain.Fake
{
    public sealed class FakeGetWorkOperation : GetWorkOperation
    {
        private readonly Work _work;

        public FakeGetWorkOperation(Work work)
        {
            _work = work;
        }

        public Work Work(string repository, string start, string end)
        {
            return _work;
        }

        public Work Work(object id)
        {
            return _work;
        }

        public IEnumerable<Work> Lasts(string repository)
        {
            return new[] {_work};
        }
    }
}