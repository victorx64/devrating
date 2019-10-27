using System.Collections.Generic;

namespace DevRating.Vcs
{
    public interface ModificationsStorage
    {
        string Insert(IEnumerable<Addition> additions, IEnumerable<Deletion> deletions);
    }
}