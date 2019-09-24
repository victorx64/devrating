using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevRating.Git
{
    public interface Repository
    {
        IEnumerable<Task<Watchdog>> FilePatches(string sha);
        string Author(string sha);
    }
}