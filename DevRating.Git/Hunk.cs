using System.Collections.Generic;

namespace DevRating.Git
{
    public sealed class Hunk
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

        public void LogAuthorChanges(AuthorsLog log)
        {
            foreach (var deletion in _deletions)
            {
                log.LogAuthorDeletion(deletion, _author, _commit);
            }

            for (var i = 0; i < _additions; i++)
            {
                log.LogAuthorAddition(_author, _commit);
            }
        }
    }
}