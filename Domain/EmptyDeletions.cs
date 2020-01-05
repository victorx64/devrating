using System.Collections.Generic;

namespace DevRating.Domain
{
    public sealed class EmptyDeletions : Deletions
    {
        public EmptyDeletions()
        {
        }

        public IEnumerable<Deletion> Items()
        {
            return new Deletion[0];
        }
    }
}