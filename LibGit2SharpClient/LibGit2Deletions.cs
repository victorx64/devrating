using System;
using System.Collections.Generic;
using DevRating.Domain;
using LibGit2Sharp;

namespace DevRating.LibGit2SharpClient
{
    public sealed class LibGit2Deletions : Deletions
    {
        private readonly string _patch;
        private readonly BlameHunkCollection _blames;

        public LibGit2Deletions(string patch, BlameHunkCollection blames)
        {
            _patch = patch;
            _blames = blames;
        }

        public IEnumerable<Deletion> Items()
        {
            var deletions = new List<Deletion>();

            foreach (var header in DeletionHeaders())
            {
                deletions.AddRange(HunkDeletions(header));
            }

            return deletions;
        }

        private IEnumerable<string> DeletionHeaders()
        {
            foreach (var line in _patch.Split('\n'))
            {
                if (line.StartsWith("@@ "))
                {
                    yield return line.Split(' ')[1];
                }
            }
        }

        private IEnumerable<Deletion> HunkDeletions(string header)
        {
            var parts = HeaderParts(header);
            var index = Index(parts);
            var count = Count(parts);
            uint increment;

            for (var i = index; i < index + count; i += increment)
            {
                var blame = _blames.HunkForLine((int) i);

                increment = BlameHunkLastLineIndex(blame, index + count) - i;

                yield return new DefaultDeletion(blame.FinalCommit.Author.Email, increment);
            }
        }

        private uint BlameHunkLastLineIndex(BlameHunk blame, uint limit)
        {
            return Math.Min((uint) (blame.FinalStartLineNumber + blame.LineCount), limit);
        }

        private IReadOnlyList<string> HeaderParts(string header)
        {
            return header.Substring(1).Split(',');
        }

        private uint Index(IReadOnlyList<string> parts)
        {
            return Convert.ToUInt32(parts[0]) - 1;
        }

        private uint Count(IReadOnlyList<string> parts)
        {
            return parts.Count == 1 ? 1 : Convert.ToUInt32(parts[1]);
        }
    }
}