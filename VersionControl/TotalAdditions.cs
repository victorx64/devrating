// Copyright (c) 2019-present Viktor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Linq;

namespace DevRating.VersionControl
{
    public sealed class TotalAdditions : Additions
    {
        private readonly Hunks _hunks;

        public TotalAdditions(Hunks hunks)
        {
            _hunks = hunks;
        }

        public uint Count()
        {
            return (uint) _hunks.Items()
                .Sum(HunkAdditions);
        }

        private long HunkAdditions(Hunk hunk)
        {
            return hunk.Additions().Count();
        }
    }
}