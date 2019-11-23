using System.Collections.Generic;

namespace DevRating.LibGit2SharpClient
{
    internal interface Deletions
    {
        IEnumerable<Modification> Items();
    }
}