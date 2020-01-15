using System.Collections.Generic;

namespace DevRating.Domain
{
    public interface Diffs
    {
        void Insert(InsertWorkParams @params, string email, IEnumerable<Deletion> deletions);
        Database Database();
        Formula Formula();
    }
}