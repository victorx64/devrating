using System.Collections.Generic;

namespace DevRating.Domain.Fake
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