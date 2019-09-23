using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevRating.Git.Test
{
    public class FakeRepository : Repository
    {
        private readonly IEnumerable<Watchdog> _watchdogs;
        private readonly string _author;

        public FakeRepository(IEnumerable<Watchdog> watchdogs, string author)
        {
            _watchdogs = watchdogs;
            _author = author;
        }

        public IEnumerable<Watchdog> FilePatches(string sha)
        {
            return _watchdogs;
        }

        public string Author(string sha)
        {
            return _author;
        }
    }
}