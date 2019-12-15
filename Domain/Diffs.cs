using System.Collections.Generic;

namespace DevRating.Domain
{
    public interface Diffs
    {
        void Insert(string repository, string start, string end, string email, uint additions,
            IDictionary<string, uint> deletions);
        Database Database();
        Formula Formula();
    }
}