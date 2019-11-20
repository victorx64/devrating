using System.Collections.Generic;

namespace DevRating.Domain
{
    public interface WorksRepository
    {
        void Add(WorkKey key, string email, uint additions, IDictionary<string, uint> deletions);
        Work Work(WorkKey key);
        bool Exist(WorkKey key);
    }
}