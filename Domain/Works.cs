using System.Collections.Generic;

namespace DevRating.Domain
{
    public interface Works
    {
        void Add(WorkKey key, Modification addition, IEnumerable<Modification> deletions);
        Work Work(WorkKey key);
        bool Exist(WorkKey key);
    }
}