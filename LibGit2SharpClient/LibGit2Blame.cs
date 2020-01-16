using DevRating.VersionControl;
using LibGit2Sharp;

namespace DevRating.LibGit2SharpClient
{
    public sealed class LibGit2Blame : Blame
    {
        private readonly BlameHunk _hunk;

        public LibGit2Blame(BlameHunk hunk)
        {
            _hunk = hunk;
        }

        public string AuthorEmail()
        {
            return _hunk.FinalCommit.Author.Email;
        }

        public uint StartLineNumber()
        {
            return (uint) _hunk.FinalStartLineNumber;
        }

        public uint LineCount()
        {
            return (uint) _hunk.LineCount;
        }

        public bool ContainsLine(uint line)
        {
            return _hunk.ContainsLine((int) line);
        }
    }
}