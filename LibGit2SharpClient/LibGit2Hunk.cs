using DevRating.VersionControl;
using LibGit2Sharp;

namespace DevRating.LibGit2SharpClient
{
    public sealed class LibGit2Hunk : Hunk
    {
        private readonly Deletions _deletions;
        private readonly Additions _additions;

        public LibGit2Hunk(string patch, BlameHunkCollection blames)
            : this(new VersionControlDeletions(patch, new LibGit2Blames(blames)), new VersionControlAdditions(patch))
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