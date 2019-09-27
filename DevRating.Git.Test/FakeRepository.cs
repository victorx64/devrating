using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevRating.Git.Test
{
    public class FakeRepository : Repository
    {
        private readonly IEnumerable<Task<Watchdog>> _watchdogs;
        private readonly string _author;

        public FakeRepository(IEnumerable<Watchdog> watchdogs, string author)
        {
            _watchdogs = watchdogs.Select(Task.FromResult);
            _author = author;
        }

        public IEnumerable<Task<Watchdog>> FilePatches(string sha)
        {
            return _watchdogs;
        }

        public string Author(string sha)
        {
            return _author;
        }
    }
}