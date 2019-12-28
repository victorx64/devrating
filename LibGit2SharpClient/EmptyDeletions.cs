using System.Collections.Generic;

namespace DevRating.LibGit2SharpClient
{
    internal sealed class EmptyDeletions : Deletions
    {
        public EmptyDeletions()
        {
        }

        public IEnumerable<Modification> Items()
        {
            return new Modification[0];
        }
    }
}