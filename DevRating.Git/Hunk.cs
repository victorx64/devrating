using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevRating.Git
{
    public class Hunk : AuthorChangesCollection
    {
        private readonly Author _author;
        private readonly IEnumerable<Author> _deletions;
        private readonly int _additions;
        private readonly string _commit;

        public Hunk(Author author, IEnumerable<Author> deletions, int additions, string commit)
        {
            _author = author;
            _deletions = deletions;
            _additions = additions;
            _commit = commit;
        }

        public Task ExtendAuthorChanges(AuthorChanges changes, Author empty)
        {
            foreach (var deletion in _deletions)
            {
                changes.AddChange(deletion, _author, _commit);
            }

            for (var i = 0; i < _additions; i++)
            {
                changes.AddChange(empty, _author, _commit);
            }
            
            return Task.CompletedTask;
        }
    }
}