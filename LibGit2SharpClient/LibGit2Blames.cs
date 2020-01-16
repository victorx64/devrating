using DevRating.VersionControl;
using LibGit2Sharp;

namespace DevRating.LibGit2SharpClient
{
    public sealed class LibGit2Blames : Blames
    {
        private readonly BlameHunkCollection _collection;

        public LibGit2Blames(BlameHunkCollection collection)
        {
            _collection = collection;
        }

        public Blame HunkForLine(int line)
        {
            return new LibGit2Blame(_collection.HunkForLine(line));
        }
    }
}