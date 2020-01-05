using System.Collections.Generic;

namespace DevRating.Domain
{
    public interface Hunks
    {
        IEnumerable<Hunk> Items();
    }
}