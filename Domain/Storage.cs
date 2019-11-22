using System.Collections.Generic;

namespace DevRating.Domain
{
    public interface Storage
    {
        void AddWork(WorkKey key, string email, uint additions, IDictionary<string, uint> deletions);
        Work Work(WorkKey key);
        bool WorkExist(WorkKey key);
        IEnumerable<Author> TopAuthors();
    }
}