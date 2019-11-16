using System.Collections.Generic;
using DevRating.Domain;

namespace DevRating.LibGit2SharpClient
{
    internal interface Deletions
    {
        IEnumerable<Modification> Items();
    }
}