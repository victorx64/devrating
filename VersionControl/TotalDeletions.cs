// Copyright (c) 2019-present Viktor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;
using DevRating.DefaultObject;
using DevRating.Domain;

namespace DevRating.VersionControl
{
    public sealed class TotalDeletions : Deletions
    {
        private readonly Patches _patches;

        public TotalDeletions(Patches patches)
        {
            _patches = patches;
        }

        public IEnumerable<Deletion> Items()
        {
            return _patches.Items()
                .SelectMany(HunkDeletions)
                .GroupBy(DeletionAuthor)
                .Select(Deletion);
        }

        private Deletion Deletion(IGrouping<string, Deletion> grouping)
        {
            return new DefaultDeletion(
                DeletionsAuthor(grouping),
                DeletionsCountsSum(grouping),
                IgnoredDeletionsCountsSum(grouping)
            );
        }

        private IEnumerable<Deletion> HunkDeletions(FilePatch hunk)
        {
            return hunk.Deletions().Items();
        }

        private string DeletionAuthor(Deletion deletion)
        {
            return deletion.Email().ToLowerInvariant();
        }

        private string DeletionsAuthor(IGrouping<string, Deletion> grouping)
        {
            return grouping.Key;
        }

        private uint DeletionsCountsSum(IEnumerable<Deletion> grouping)
        {
            return (uint) grouping.Sum(DeletionsCount);
        }

        private long DeletionsCount(Deletion deletion)
        {
            return deletion.Counted();
        }

        private uint IgnoredDeletionsCountsSum(IEnumerable<Deletion> grouping)
        {
            return (uint) grouping.Sum(IgnoredDeletionsCount);
        }

        private long IgnoredDeletionsCount(Deletion deletion)
        {
            return deletion.Ignored();
        }
    }
}