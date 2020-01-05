using System.Collections.Generic;

namespace DevRating.Domain
{
    public sealed class CachedHunks : Hunks
    {
        private readonly Hunks _origin;
        private IEnumerable<Hunk>? _items;

        public CachedHunks(Hunks origin)
        {
            _origin = origin;
        }

        public IEnumerable<Hunk> Items()
        {
            lock (_origin)
            {
                return _items ??= _origin.Items();
            }
        }
    }
}