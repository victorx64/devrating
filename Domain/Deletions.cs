using System.Collections.Generic;

namespace DevRating.Domain
{
    public interface Deletions
    {
        IEnumerable<Deletion> Items();
    }
}