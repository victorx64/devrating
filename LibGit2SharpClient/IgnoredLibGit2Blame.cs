using System;
using DevRating.DefaultObject;
using DevRating.Domain;
using DevRating.VersionControl;
using LibGit2Sharp;

namespace DevRating.LibGit2SharpClient
{
    public sealed class IgnoredLibGit2Blame : Blame
    {
        private readonly BlameHunk _hunk;

        public IgnoredLibGit2Blame(BlameHunk hunk)
        {
            _hunk = hunk;
        }

        public bool ContainsLine(uint line)
        {
            return _hunk.ContainsLine((int) line);
        }

        public Deletion Deletion(uint i, uint limit)
        {
            return new DefaultDeletion(
                _hunk.FinalCommit.Author.Email,
                0,
                Math.Min((uint) _hunk.FinalStartLineNumber + (uint) _hunk.LineCount, limit) - i
            );
        }
    }
}