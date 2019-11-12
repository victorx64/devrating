using System.Collections.Generic;

namespace DevRating.Domain.Git
{
    public interface Repository
    {
        IEnumerable<Commit> Commits(string since, string until);
    }
}