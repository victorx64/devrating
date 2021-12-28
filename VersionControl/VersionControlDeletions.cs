// Copyright (c) 2019-present Victor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using DevRating.Domain;

namespace DevRating.VersionControl
{
    public sealed class VersionControlDeletions : Deletions
    {
        private readonly IEnumerable<string> _lines;
        private readonly AFileBlames _blames;

        public VersionControlDeletions(string patch, AFileBlames blames)
            : this (patch.Split('\n'), blames)
        {
        }

        public VersionControlDeletions(IEnumerable<string> patch, AFileBlames blames)
        {
            _lines = patch;
            _blames = blames;
        }

        public IEnumerable<Deletion> Items()
        {
            var deletions = new List<Deletion>();

            foreach (var header in HunkHeaders())
            {
                deletions.AddRange(HunkDeletions(header));
            }

            return deletions;
        }

        private IEnumerable<string> HunkHeaders()
        {
            foreach (var line in _lines)
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
                var deletion = _blames.AtLine(i).SubDeletion(i, index + count);

                increment = deletion.Counted() + deletion.Ignored();

                yield return deletion;
            }
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