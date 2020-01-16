using System.Collections.Generic;
using DevRating.Domain;

namespace DevRating.VersionControl.Fake
{
    public sealed class FakeDeletions : Deletions
    {
        private readonly IEnumerable<Deletion> _items;

        public FakeDeletions(IEnumerable<Deletion> items)
        {
            _items = items;
        }

        public IEnumerable<Deletion> Items()
        {
            return _items;
        }
    }
}