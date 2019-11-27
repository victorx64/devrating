using System.Collections.Generic;

namespace DevRating.Domain
{
    public interface Storage
    {
        void AddWork(Diff diff, string email, uint additions, IDictionary<string, uint> deletions);
        Work Work(Diff diff);
        bool WorkExist(Diff diff);
        IEnumerable<Author> TopAuthors();
    }
}