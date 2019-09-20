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

        public Task WriteInto(Log log)
        {
            foreach (var deletion in _deletions)
            {
                log.LogDeletion(deletion);
            }

            for (var i = 0; i < _additions; i++)
            {
                log.LogAddition();
            }
            
            return Task.CompletedTask;
        }
    }
}