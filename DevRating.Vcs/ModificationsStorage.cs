using System.Collections.Generic;

namespace DevRating.Vcs
{
    public interface ModificationsStorage
    {
        void Insert(IEnumerable<Addition> additions, IEnumerable<Deletion> deletions);
    }
}