using System.Collections.Generic;

namespace DevRating.Domain
{
    public interface Diffs
    {
        void Insert(string repository, string start, string end, string email, uint additions,
            IEnumerable<Deletion> deletions);

        Database Database();
        Formula Formula();

        void Insert(string repository, string link, string start, string end, string email, uint additions,
            IEnumerable<Deletion> deletions);
    }
}