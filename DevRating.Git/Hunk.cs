using System.Collections.Generic;
using System.Threading.Tasks;

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

        public Task WriteInto(History history)
        {
            foreach (var deletion in _deletions)
            {
                history.LogDeletion(deletion);
            }

            history.LogAdditions(_additions);
            
            return Task.CompletedTask;
        }
    }
}