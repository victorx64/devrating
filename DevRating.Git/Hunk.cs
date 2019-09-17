using System.Collections.Generic;
using System.Linq;

namespace DevRating.Git
{
    internal sealed class Hunk
    {
        private readonly string _author;
        private readonly IEnumerable<string> _deletions;
        private readonly int _additions;
        private readonly string _commit;

        public Hunk(string author, IEnumerable<string> deletions, int additions, string commit)
        {
            _author = author;
            _deletions = deletions;
            _additions = additions;
            _commit = commit;
        }

        public void WriteInto(Log log)
        {
            var deletions = _deletions.GroupBy(d => d); 
            
            foreach (var deletion in deletions)
            {
                log.LogDeletion(deletion.Count(), deletion.Key, _author, _commit);
            }

            log.LogAddition(_additions, _author, _commit);
        }
    }
}