using System.Collections.Generic;

namespace DevRating.Git
{
    public interface Repository
    {
        IEnumerable<Watchdog> FilePatches(string sha);
        string Author(string sha);
    }
}