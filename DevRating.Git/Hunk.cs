using System.Collections.Generic;

namespace DevRating.Git
{
    internal sealed class Hunk : Watchdog
    {
        private readonly IEnumerable<string> _deletions;
        private readonly int _additions;

        public Hunk(IEnumerable<string> deletions, int additions)
        {
            _deletions = deletions;
            _additions = additions;
        }

        public void WriteInto(Modifications modifications)
        {
            foreach (var deletion in _deletions)
            {
                modifications.AddDeletion(deletion);
            }

            modifications.AddAdditions(_additions);
        }
    }
}