// Copyright (c) 2019-present Victor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Linq;

namespace DevRating.VersionControl
{
    public sealed class TotalAdditions : Additions
    {
        private readonly Patches _patches;

        public TotalAdditions(Patches patches)
        {
            _patches = patches;
        }

        public uint Count()
        {
            return (uint) _patches.Items()
                .Sum(PatchAdditions);
        }

        private long PatchAdditions(FilePatch patch)
        {
            return patch.Additions().Count();
        }
    }
}