using System.Collections.Generic;

namespace DevRating.Git
{
    public class Patch
    {
        private readonly Author _author;
        private readonly IEnumerable<Author> _deletions;
        private readonly int _additions;
        private readonly string _commit;

        public Patch(Author author, IEnumerable<Author> deletions, int additions, string commit)
        {
            _author = author;
            _deletions = deletions;
            _additions = additions;
            _commit = commit;
        }
    }
}