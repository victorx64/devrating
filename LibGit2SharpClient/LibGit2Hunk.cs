using LibGit2Sharp;

namespace DevRating.LibGit2SharpClient
{
    internal sealed class LibGit2Hunk : Hunk
    {
        private readonly Deletions _deletions;
        private readonly Additions _additions;

        public LibGit2Hunk(IRepository repository, string patch, BlameHunkCollection blames)
            : this(new LibGit2Deletions(repository, patch, blames), new LibGit2Additions(patch))
        {
        }

        public LibGit2Hunk(Deletions deletions, Additions additions)
        {
            _deletions = deletions;
            _additions = additions;
        }

        public Deletions Deletions()
        {
            return _deletions;
        }

        public Additions Additions()
        {
            return _additions;
        }
    }
}