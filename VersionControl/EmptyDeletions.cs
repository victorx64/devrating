using System.Collections.Generic;
using DevRating.Domain;

namespace DevRating.VersionControl
{
    public sealed class EmptyDeletions : Deletions
    {
        public IEnumerable<Deletion> Items()
        {
            return new Deletion[0];
        }
    }
}