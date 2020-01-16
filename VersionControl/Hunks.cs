using System.Collections.Generic;

namespace DevRating.VersionControl
{
    public interface Hunks
    {
        IEnumerable<Hunk> Items();
    }
}