using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevRating.Git
{
    public sealed class Hunk : AuthorChangesCollection
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

        public Task ExtendAuthorChanges(AuthorChanges changes)
        {
            foreach (var deletion in _deletions)
            {
                changes.AddChange(deletion, _author, _commit);
            }

            for (var i = 0; i < _additions; i++)
            {
                changes.AddChange(_author, _commit);
            }
            
            return Task.CompletedTask;
        }
    }
}