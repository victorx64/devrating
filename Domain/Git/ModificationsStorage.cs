using System.Collections.Generic;

namespace DevRating.Domain.Git
{
    public interface ModificationsStorage
    {
        string Insert(IEnumerable<Addition> additions, IEnumerable<Deletion> deletions); // TODO Return entities instead
    }
}