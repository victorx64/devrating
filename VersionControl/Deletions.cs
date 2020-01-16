using System.Collections.Generic;
using DevRating.Domain;

namespace DevRating.VersionControl
{
    public interface Deletions
    {
        IEnumerable<Deletion> Items();
    }
}