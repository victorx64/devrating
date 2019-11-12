using System.Collections.Generic;

namespace DevRating.Domain
{
    public interface Repository
    {
        IEnumerable<Commit> Commits(string since, string until);
    }
}