using System;
using System.Collections.Generic;
using DevRating.Domain;

namespace DevRating.VersionControl
{
    public sealed class VersionControlDeletions : Deletions
    {
        private readonly string _patch;
        private readonly Blames _blames;

        public VersionControlDeletions(string patch, Blames blames)
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
                var blame = _blames.HunkForLine(i);

                increment = BlameHunkLastLineIndex(blame, index + count) - i;

                yield return new DefaultDeletion(blame.AuthorEmail(), increment);
            }
        }

        private uint BlameHunkLastLineIndex(Blame blame, uint limit)
        {
            return Math.Min(blame.StartLineNumber() + blame.LineCount(), limit);
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