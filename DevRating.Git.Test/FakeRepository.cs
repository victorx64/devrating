using System.Collections.Generic;

namespace DevRating.Git.Test
{
    public class FakeRepository : Repository
    {
        public IEnumerable<Watchdog> FilePatches(string sha)
        {
            throw new System.NotImplementedException();
        }

        public string Author(string sha)
        {
            throw new System.NotImplementedException();
        }
    }
}